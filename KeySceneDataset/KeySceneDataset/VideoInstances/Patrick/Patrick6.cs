/// Patrick6.cs holds the data required to evaluate the video in the dataset
/// known as "Patrick6".
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
    class Patrick6 : VideoResource
    {
        public Patrick6() : base(Dataset.Videos.Patrick6, 25.11)
        {
            this.AddEmotionFeedback(contemptuous: 100);
            this.AddEmotionFeedback(happy: 25, contemptuous: 75);
            this.AddEmotionFeedback(angry: 50, surprised: 25, contemptuous: 25);

            this.AddSuggestedScene(0, 9);
            this.AddSuggestedScene(10.5, 1.5);
            this.AddSuggestedScene(12.5, 1);
            this.AddSuggestedScene(16, 6);

            this.AddConsensusScene(1.5, 2.5);
            this.AddConsensusScene(7, 1.5);
        }
    }
}
