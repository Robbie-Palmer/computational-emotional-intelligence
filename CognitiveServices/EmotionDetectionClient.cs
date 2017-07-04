/// EmotionDetectionClient.cs is an abstract class, which provides the
/// basis for implementations which will utilise the information
/// retrieved from Microsoft's Emotion API.
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

namespace CognitiveServices
{
    using Microsoft.ProjectOxford.Common.Contract;
    using Microsoft.ProjectOxford.Emotion;
    using Microsoft.ProjectOxford.Emotion.Contract;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Threading.Tasks;

    public abstract class EmotionDetectionClient
    {
        public enum Emotion { Neutrality, Happiness, Sadness, Surprise, Anger, Contempt, Disgust, Fear }
        private EmotionServiceClient client;

        public EmotionDetectionClient()
        {
            var subscriptionKey = ConfigurationManager.AppSettings["Emotion-API-Sub-Key"];
            client = new EmotionServiceClient(subscriptionKey);
        }

        protected async Task<VideoAggregateRecognitionResult> GetAggregateResult(string videoFilePath)
        {
            using (Stream stream = File.OpenRead(videoFilePath))
            {
                // Send video to API
                var videoOperation = await client.RecognizeInVideoAsync(stream);
                VideoOperationResult result;
                var waitTime = TimeSpan.FromSeconds(20);

                do
                {
                    // Wait until video processed and get result
                    await Task.Delay(waitTime);
                    result = await client.GetOperationResultAsync(videoOperation);
                }
                while (result.Status != VideoOperationStatus.Succeeded && result.Status != VideoOperationStatus.Failed);

                if (result.Status == VideoOperationStatus.Failed)
                {
                    Console.WriteLine("Emotion analysis failed, because: {0}", result.Message);
                    return null;
                }

                return ((VideoOperationInfoResult<VideoAggregateRecognitionResult>)result).ProcessingResult;
            }
        }
    }
}
