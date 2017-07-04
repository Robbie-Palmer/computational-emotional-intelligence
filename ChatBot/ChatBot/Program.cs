/// Program.cs is responsible for executing the workflow to iterate over
/// the videos in the test dataset, call numerous cognitive services
/// APIs to gather applicable information regarding the supposed
/// communication, and provide a response to each video.
/// 
/// Copyright(C) <2017>  <Robert Palmer>
/// This program is free software: you can redistribute it and/or modify
/// it under the terms of the GNU General Public License as published by
/// the Free Software Foundation, either version 3 of the License, or
/// (at your option) any later version.
///
/// This program is distributed in the hope that it will be useful,
/// but WITHOUT ANY WARRANTY; without even the implied warranty of
/// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.See the
/// GNU General Public License for more details.
///
/// You should have received a copy of the GNU General Public License
/// along with this program.If not, see<http://www.gnu.org/licenses/>.

namespace ChatBot
{
    using System;
    using System.IO;
    using CognitiveServices;
    using static CognitiveServices.EmotionDetectionClient;
    using CognitiveServices.Parsing_Classes.LUIS_Intent;
    using static CognitiveServices.TextAnalyticsClient;
    using System.Collections.Generic;
    using System.Configuration;
    using Newtonsoft.Json;
    using System.Threading.Tasks;

    class Program
    {
        static void Main(string[] args)
        {
            var inputFolder = ConfigurationManager.AppSettings["DatasetFolder"];

            var files = Directory.GetFiles(inputFolder);

            var emotionClient = new EmotionRecogniser();
            var speechClient = new SpeechIntentClient();
            var textAnalyser = new TextAnalyticsClient();
            var textToSpeechClient = new TextToSpeechClient();

            var cache = GetLocalCache();

            foreach (var path in files)
            {
                if (!Path.GetExtension(path).Equals(".mp4"))
                    continue;

                var fileName = Path.GetFileNameWithoutExtension(path);

                Console.WriteLine("Processing: " + fileName);
                
                // Start processing the video file to determine facial expression of emotion
                var recogniseEmotionsTask = GetOverallEmotion(emotionClient, cache, path);

                // Process audio file, and determine intent
                speechClient.SendAudio(path);
                var speechTask = speechClient.DetermineSpeechAndIntent();
                speechTask.Wait();
                var luisResponse = speechTask.Result;

                // Prepare the Text Analytics client with the text parsed from the audio
                textAnalyser.SetData(luisResponse.query);

                // Send the text for sentiment and key phrase analysis
                var sentimentTask = textAnalyser.GetSentenceSentiment();

                // Wait for the text analysis and emotion analysis to complete
                sentimentTask.Wait();
                recogniseEmotionsTask.Wait();

                LogResults(luisResponse.query, luisResponse.intents[0], recogniseEmotionsTask.Result, sentimentTask.Result);
                
                // TODO: Create code which takes an intent, entities, actions and focus of those actions, and prints an appropriate response 
                var response = ResponseGenerator.GetResponse(luisResponse.intents[0].intent, recogniseEmotionsTask.Result, sentimentTask.Result);

                textToSpeechClient.SetTextToSay(response);
                textToSpeechClient.ProcessTextToSpeech().Wait();

                Console.WriteLine("\nResponse: ");
                Console.WriteLine(response);
                Console.WriteLine();
                Console.WriteLine("Press any key to hear the audio response.");
                //Console.ReadKey();

                textToSpeechClient.Speak();

                Console.WriteLine("--------------");
                Console.WriteLine("Finished processing. Press any key to find and process next file.");
                //Console.ReadKey();

                textAnalyser.ResetData();
                textToSpeechClient.Dispose();
            }

            speechClient.Dispose();

            StoreCache(cache);

            Console.WriteLine("Processed all files. Press any key to exit program.");

            Console.ReadKey();
        }

        private static async Task<Emotion> GetOverallEmotion(EmotionRecogniser emotionClient, Dictionary<string, Emotion> cache, string filePath)
        {
            Emotion emotion;

            if (cache.TryGetValue(filePath, out emotion))
                return emotion;

            emotion = await emotionClient.GetOverallEmotion(filePath);

            cache[filePath] = emotion;

            return emotion;
        }

        private static void LogResults(string query, Intent intent, Emotion emotion, Sentiment sentiment)
        {
            var intentLikelihood = string.Format("{0:0.00}", intent.score);

            Console.WriteLine("You said: {0}", query);
            Console.WriteLine("I am {0}% sure your intention was {1}", intentLikelihood, intent.intent);
            Console.WriteLine("The emotion displayed was {0}", emotion);
            Console.WriteLine("The sentiment of the chosen words was {0}", sentiment);
        }
        
        private static Dictionary<string, Emotion> GetLocalCache()
        {
            var cacheFilePath = ConfigurationManager.AppSettings["CacheFilePath"];

            if (!File.Exists(cacheFilePath))
                return new Dictionary<string, Emotion>();

            var serialisedResult = File.ReadAllText(cacheFilePath);
            return JsonConvert.DeserializeObject<Dictionary<string, Emotion>>(serialisedResult);
        }

        private static void StoreCache(Dictionary<string, Emotion> cache)
        {
            var cacheFilePath = ConfigurationManager.AppSettings["CacheFilePath"];

            var serialisedResult = JsonConvert.SerializeObject(cache);

            var directoryPath = Path.GetDirectoryName(cacheFilePath);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            File.WriteAllText(cacheFilePath, serialisedResult);
        }
    }
}
