/// EmotionStrengthDetector.cs is an implementation of EmotionDetectionClient,
/// which is designed to take a video file, and translate it into a sequence
/// of emotion frames, which detail the emotion and strength of emotion
/// being displayed at each frame.
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class EmotionStrengthDetector : EmotionDetectionClient
    {
        public async Task<List<EmotionFrame>> GetFrameEmotionStrengthAndTime(string videoFilePath)
        {
            try
            {
                var emotionResults = await GetAggregateResult(videoFilePath);
                var strengthTimePairs = new List<EmotionFrame>();

                foreach (var fragment in emotionResults.Fragments)
                {
                    if (fragment == null || fragment.Events == null)
                        continue;

                    var frameTime = fragment.Start * emotionResults.Framerate;
                    var interval = fragment.Interval * emotionResults.Framerate ?? 0;

                    foreach (var eventArray in fragment.Events)
                    {
                        if (eventArray.Length == 0)
                            continue;

                        foreach (var emotionEvent in eventArray)
                        {
                            if (emotionEvent == null)
                                continue;

                            var topScorePair = emotionEvent.WindowMeanScores.ToRankedList().First();
                            Emotion topEmotion;
                            Enum.TryParse(topScorePair.Key, out topEmotion);

                            strengthTimePairs.Add(new EmotionFrame(topEmotion, topScorePair.Value, emotionEvent.WindowMeanScores.Neutral, frameTime, interval));

                            //Console.WriteLine("Emotion Strength = {0}\tFrom : {1} to {2}", emotionStrength, frameTime, frameTime + interval);
                        }

                        frameTime += interval;
                    }
                }

                return strengthTimePairs;
            }
            catch(Exception exception)
            {
                Console.Error.WriteLine(exception.ToString());
                return null;
            }
        }

        public class EmotionFrame
        {
            public Emotion Emotion { get; }
            public double EmotionStrength { get; }
            public double NeutralStrength { get; }
            public double StartTime { get; }
            public double Interval { get; }

            public EmotionFrame(Emotion emotion, double emotionStrength, double neutralStrength, double startTime, double interval)
            {
                Emotion = emotion;
                EmotionStrength = emotionStrength;
                NeutralStrength = neutralStrength;
                StartTime = startTime;
                Interval = interval;
            }
        }
    }
}
