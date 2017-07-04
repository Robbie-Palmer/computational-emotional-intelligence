/// UnlikelyEventModel.cs provides a function which based on
/// transitions between scenes, will extract those which correspond to
/// unlikely occurences.
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
    using System;
    using System.Collections.Generic;
    using static CognitiveServices.EmotionStrengthDetector;

    class UnlikelyEventModel
    {
        public delegate double LikelihoodCalculator(EmotionFrame currentFrame, EmotionFrame nextFrame);

        public static IList<EmotionFrame> GetUnlikelyScenes(IList<EmotionFrame> allScenes, double threshold, LikelihoodCalculator CalcLikelihood)
        {
            if(allScenes == null)
                return new List<EmotionFrame>();

            var scenesWithLikelihoods = new List<Tuple<EmotionFrame, double>>();

            for (var i = 0; i < allScenes.Count - 1; i++)
                scenesWithLikelihoods.Add(new Tuple<EmotionFrame, double>(allScenes[i], CalcLikelihood(allScenes[i], allScenes[i + 1])));

            var keyScenes = new List<EmotionFrame>();

            var secondLastSceneIdx = scenesWithLikelihoods.Count - 1;
            for (var i = 0; i < secondLastSceneIdx; i++)
            {
                if (scenesWithLikelihoods[i].Item2 <= threshold)
                    keyScenes.Add(scenesWithLikelihoods[i].Item1);
            }

            // Cannot evaluate the last scene, so include if second last scene deemed important
            if (scenesWithLikelihoods[secondLastSceneIdx].Item2 <= threshold)
            {
                keyScenes.Add(scenesWithLikelihoods[secondLastSceneIdx].Item1);
                keyScenes.Add(allScenes[secondLastSceneIdx + 1]);
            }

            return keyScenes;
        }
    }
}
