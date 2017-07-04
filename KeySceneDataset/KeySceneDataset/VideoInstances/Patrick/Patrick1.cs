/// Patrick1.cs holds the data required to evaluate the video in the dataset
/// known as "Patrick1".
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
    class Patrick1 : VideoResource
    {
        public Patrick1() : base(Dataset.Videos.Patrick1, 45.67)
        {
            this.AddEmotionFeedback(angry: 100 * (1D / 3D), contemptuous: 100 * (1D / 3D), disgusted: 100 * (1D / 3D));
            this.AddEmotionFeedback(angry: 40, contemptuous: 40, disgusted: 20);
            this.AddEmotionFeedback(angry: 5, contemptuous: 75, disgusted: 20);
            this.AddEmotionFeedback(contemptuous: 35, disgusted: 65);

            this.AddSuggestedScene(8, 2);
            this.AddSuggestedScene(12, 3);
            this.AddSuggestedScene(15.5, 4.5);
            this.AddSuggestedScene(25.5, 4);
            this.AddSuggestedScene(33.5, 2);
            this.AddSuggestedScene(37, 2);
            this.AddSuggestedScene(39.5, 6.17);

            this.AddConsensusScene(8, 2);
            this.AddConsensusScene(15.5, 3.5);
            this.AddConsensusScene(26.5, 1);
            this.AddConsensusScene(28.5, 1);
            this.AddConsensusScene(37, 2);
            this.AddConsensusScene(40, 3);
        }
    }
}
