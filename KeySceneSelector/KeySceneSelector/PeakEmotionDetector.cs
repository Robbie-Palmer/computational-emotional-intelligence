/// PeakEmotionDetector.cs inherits from KeySceneSelector.cs and implements
/// the GetKeyFrames method, to return frames which display the most emotion.
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
    using static CognitiveServices.EmotionStrengthDetector;

    public class PeakEmotionDetector : KeySceneSelector
    {
        public PeakEmotionDetector(string filePath) : base(filePath)
        {
        }

        protected override IList<EmotionFrame> GetKeyFrames(List<EmotionFrame> allFrames, double topPercent)
        {
            // Order from least neutral to most neutral
            allFrames.Sort((x, y) => x.NeutralStrength.CompareTo(y.NeutralStrength));

            var sectionLength = (int)(allFrames.Count * topPercent);
            var framesInTopPercent = allFrames.GetRange(0, sectionLength);

            framesInTopPercent.Sort((x, y) => x.StartTime.CompareTo(y.StartTime));

            return framesInTopPercent;
        }
    }
}
