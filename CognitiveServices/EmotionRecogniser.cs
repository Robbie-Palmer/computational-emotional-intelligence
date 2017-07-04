/// EmotionRecogniser.cs is an implementation of EmotionDetectionClient,
/// which is designed to take a video file, and return the most relevant
/// emotion that was displayed.
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
    using Microsoft.ProjectOxford.Emotion.Contract;
    using System;
    using System.Threading.Tasks;

    public class EmotionRecogniser : EmotionDetectionClient
    {
        private const double NonNeutralConfidenceThreshold = 0.15;

        public async Task<Emotion> GetOverallEmotion(string videoFilePath)
        {
            try
            {
                var aggregateResult = await GetAggregateResult(videoFilePath);

                if (aggregateResult == null)
                {
                    Console.WriteLine("Emotion analysis failed: Proceed with default response for neutral expression");
                    return Emotion.Neutrality;
                }

                Console.WriteLine("Emotion recognition results:");

                var totEmotionScores = GetTotalEmotionScores(aggregateResult);

                foreach (var emoshScore in totEmotionScores)
                    Console.WriteLine(emoshScore.Emotion + " " + emoshScore.Score);

                return ChooseTopEmotion(totEmotionScores);
            }
            catch (Exception exception)
            {
                Console.Error.WriteLine(exception.ToString());
                return Emotion.Neutrality;
            }
        }
        
        private static Emotion ChooseTopEmotion(EmotionalScore[] totEmotionScores)
        {
            var maxValue = 0.0;
            var secondMaxValue = 0.0;

            var summedScores = 0.0;

            // Default is neutral
            Emotion maxEmotion = Emotion.Neutrality;
            Emotion secondMaxEmotion = Emotion.Neutrality;

            foreach (var emotionScore in totEmotionScores)
            {
                if (emotionScore.Score > maxValue)
                {
                    maxValue = emotionScore.Score;
                    maxEmotion = emotionScore.Emotion;
                }
                else if (emotionScore.Score > secondMaxValue)
                {
                    secondMaxValue = emotionScore.Score;
                    secondMaxEmotion = emotionScore.Emotion;
                }

                summedScores += emotionScore.Score;
            }

            Console.WriteLine("Max Emotion = {0}, {1}", maxEmotion, maxValue);
            Console.WriteLine("Second Max Emotion = {0}, {1}", secondMaxEmotion, secondMaxValue);
            Console.WriteLine("Second Max percentage of total = {0}", (secondMaxValue / summedScores));

            // If most common is not neutral, return it, or if second most popular's relative score is below threshold
            if (maxEmotion != Emotion.Neutrality || (secondMaxValue / summedScores) < NonNeutralConfidenceThreshold)
                return maxEmotion;

            return secondMaxEmotion;
        }

        private static EmotionalScore[] GetTotalEmotionScores(VideoAggregateRecognitionResult emotionResults)
        {
            var totEmotionScores = new EmotionalScore[]
            {
                new EmotionalScore(0.0, Emotion.Neutrality),
                new EmotionalScore(0.0, Emotion.Happiness),
                new EmotionalScore(0.0, Emotion.Sadness),
                new EmotionalScore(0.0, Emotion.Surprise),
                new EmotionalScore(0.0, Emotion.Anger),
                new EmotionalScore(0.0, Emotion.Contempt),
                new EmotionalScore(0.0, Emotion.Disgust),
                new EmotionalScore(0.0, Emotion.Fear)
            };

            foreach (var fragment in emotionResults.Fragments)
            {
                if (fragment == null || fragment.Events == null)
                    continue;

                foreach (var eventArray in fragment.Events)
                {
                    if (eventArray.Length == 0)
                        continue;

                    foreach (var emotionEvent in eventArray)
                    {
                        if (emotionEvent == null)
                            continue;

                        totEmotionScores[0].Score += emotionEvent.WindowMeanScores.Neutral;
                        totEmotionScores[1].Score += emotionEvent.WindowMeanScores.Happiness;
                        totEmotionScores[2].Score += emotionEvent.WindowMeanScores.Sadness;
                        totEmotionScores[3].Score += emotionEvent.WindowMeanScores.Surprise;

                        totEmotionScores[4].Score += emotionEvent.WindowMeanScores.Anger;
                        totEmotionScores[5].Score += emotionEvent.WindowMeanScores.Contempt;
                        totEmotionScores[6].Score += emotionEvent.WindowMeanScores.Disgust;
                        totEmotionScores[7].Score += emotionEvent.WindowMeanScores.Fear;
                    }
                }
            }

            return totEmotionScores;
        }

        private class EmotionalScore
        {
            public double Score { get; set; }
            public Emotion Emotion { get; set; }

            public EmotionalScore(double score, Emotion emotion)
            {
                Score = score;
                Emotion = emotion;
            }
        }
    }
}
