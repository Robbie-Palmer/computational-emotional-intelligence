/// Mark2.cs holds the data required to evaluate the video in the dataset
/// known as "Mark2".
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
    class Mark2 : VideoResource
    {
        public Mark2() : base(Dataset.Videos.Mark2, 40.77)
        {
            this.AddEmotionFeedback(sad: 25, angry: 25, contemptuous: 50);
            this.AddEmotionFeedback(happy: 50, contemptuous: 50);
            this.AddEmotionFeedback(angry: 10, contemptuous: 70, disgusted: 20);
            this.AddEmotionFeedback(sad: 20, contemptuous: 80);

            this.AddSuggestedScene(4.5, 1);
            this.AddSuggestedScene(9.5, 3.5);
            this.AddSuggestedScene(14, 8.5);
            this.AddSuggestedScene(26.5, 7);
            this.AddSuggestedScene(36.5, 4.27);

            this.AddConsensusScene(9.5, 1.5);
            this.AddConsensusScene(14, 3);
        }
    }
}
