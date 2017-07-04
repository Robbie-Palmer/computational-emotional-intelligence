/// Mark3.cs holds the data required to evaluate the video in the dataset
/// known as "Mark3".
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
    class Mark3 : VideoResource
    {
        public Mark3() : base(Dataset.Videos.Mark3, 30.78)
        {
            this.AddEmotionFeedback(happy: 100);
            this.AddEmotionFeedback(happy: 100);
            this.AddEmotionFeedback(happy: 100);
            this.AddEmotionFeedback(happy: 60, surprised: 40);

            this.AddSuggestedScene(0, 4);
            this.AddSuggestedScene(4.5, 1);
            this.AddSuggestedScene(7, 5);
            this.AddSuggestedScene(14, 3.5);
            this.AddSuggestedScene(18, 2);
            this.AddSuggestedScene(20.5, 1);
            this.AddSuggestedScene(22.5, 8.28);

            this.AddConsensusScene(14, 1);
            this.AddConsensusScene(15.5, 1);
            this.AddConsensusScene(27, 1);
            this.AddConsensusScene(28.5, 1.5);
        }
    }
}
