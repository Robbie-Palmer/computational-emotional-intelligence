/// EmotionStateTransitionMarkovChain.cs inherits from KeySceneSelector.cs
/// and implements the GetKeyFrames method, to return frames which
/// correspond with unlikely emotion state transitions.
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

namespace KeySceneSelector
{
    using System.Collections.Generic;
    using static CognitiveServices.EmotionDetectionClient;
    using static CognitiveServices.EmotionStrengthDetector;

    public class EmotionStateTransitionMarkovChain : KeySceneSelector
    {
        private const int NEUTRAL_IDX = 0;
        private const int HAPPY_IDX = 1;
        private const int SAD_IDX = 2;
        private const int ANGRY_IDX = 3;
        private const int SURPRISED_IDX = 4;
        private const int FEARFUL_IDX = 5;
        private const int CONTEMPTUOUS_IDX = 6;
        private const int DISGUSTED_IDX = 7;

        private readonly static double[,] transitionMatrix =
            {
                { 0.3, 0.15, 0.15, 0.15, 0.07, 0.07, 0.05, 0.06 },
                { 0.475, 0.475, 0, 0, 0.05, 0, 0, 0 },
                { 0.3, 0, 0.5, 0.2, 0, 0, 0, 0 },
                { 0.2, 0, 0, 0.6, 0, 0, 0.2, 0 },
                { 0.2, 0.2, 0, 0.15, 0.3, 0.10, 0, 0.05 },
                { 0, 0, 0.2, 0.2, 0, 0.6, 0, 0 },
                { 0.3, 0, 0, 0.3, 0, 0, 0.3, 0.1 },
                { 0.4, 0, 0, 0, 0, 0, 0.1, 0.5 }
            };

        public EmotionStateTransitionMarkovChain(string filePath) : base(filePath)
        {
        }

        public static ISet<double> GetSetOfTransitionLikelihoods()
        {
            var likelihoods = new HashSet<double>();
            foreach (var likelihood in transitionMatrix)
                likelihoods.Add(likelihood);

            return likelihoods;
        }
        
        protected override IList<EmotionFrame> GetKeyFrames(List<EmotionFrame> allScenes, double threshold)
        {
            return UnlikelyEventModel.GetUnlikelyScenes(allScenes, threshold, CalcLikelihood);
        }

        private static double CalcLikelihood(EmotionFrame currentFrame, EmotionFrame nextFrame)
        {
            return transitionMatrix[getEmotionIndex(currentFrame.Emotion), getEmotionIndex(nextFrame.Emotion)];
        }
        
        private static int getEmotionIndex(Emotion emotion)
        {
            switch(emotion)
            {
                case Emotion.Neutrality: return NEUTRAL_IDX;
                case Emotion.Happiness: return HAPPY_IDX;
                case Emotion.Sadness: return SAD_IDX;
                case Emotion.Anger: return ANGRY_IDX;
                case Emotion.Surprise: return SURPRISED_IDX;
                case Emotion.Fear: return FEARFUL_IDX;
                case Emotion.Contempt: return CONTEMPTUOUS_IDX;
                case Emotion.Disgust: return DISGUSTED_IDX;
            }

            return NEUTRAL_IDX;
        }
    }
}
