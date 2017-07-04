/// Chris1.cs holds the data required to evaluate the video in the dataset
/// known as "Chris1".
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

namespace KeySceneDataset.VideoInstances
{
    class Chris1 : VideoResource
    {
        public Chris1() : base(Dataset.Videos.Chris1, 20.36)
        {
            this.AddEmotionFeedback(neutral: 75, happy: 10, sad: 10, surprised: 5);
            this.AddEmotionFeedback(angry: 25, contemptuous: 25, disgusted: 50);
            this.AddEmotionFeedback(neutral: 100 * (1D / 3D), contemptuous: 100 * (1D / 3D), disgusted: 100 * (1D / 3D));
            this.AddEmotionFeedback(neutral: 90, sad: 10);

            this.AddSuggestedScene(0, 4);
            this.AddSuggestedScene(5, 4.5);
            this.AddSuggestedScene(10.5, 0.5);
            this.AddSuggestedScene(12.5, 0.5);
            this.AddSuggestedScene(14, 4);

            this.AddConsensusScene(14, 1);
        }
    }
}
