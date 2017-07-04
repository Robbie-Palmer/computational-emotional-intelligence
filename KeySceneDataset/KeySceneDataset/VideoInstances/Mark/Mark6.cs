/// Mark6.cs holds the data required to evaluate the video in the dataset
/// known as "Mark6".
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
    class Mark6 : VideoResource
    {
        public Mark6() : base(Dataset.Videos.Mark6, 36.29)
        {
            this.AddEmotionFeedback(angry: 40, surprised: 20, disgusted: 40);
            this.AddEmotionFeedback(angry: 50, surprised: 25, contemptuous: 25);
            this.AddEmotionFeedback(sad: 10, angry: 75, contemptuous: 15);
            this.AddEmotionFeedback(sad: 10, angry: 30, contemptuous: 60);

            this.AddSuggestedScene(1.5, 0.5);
            this.AddSuggestedScene(3, 2.5);
            this.AddSuggestedScene(7, 8);
            this.AddSuggestedScene(8.5, 3.5);
            this.AddSuggestedScene(14.5, 12.5);
            this.AddSuggestedScene(28, 3);
            this.AddSuggestedScene(32, 4);

            this.AddConsensusScene(3, 1);
            this.AddConsensusScene(7, 1);
        }
    }
}
