/// Mark5.cs holds the data required to evaluate the video in the dataset
/// known as "Mark5".
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
    class Mark5 : VideoResource
    {
        public Mark5() : base(Dataset.Videos.Mark5, 36.93)
        {
            this.AddEmotionFeedback(neutral: 50, contemptuous: 50);
            this.AddEmotionFeedback(neutral: 50, contemptuous: 50);
            this.AddEmotionFeedback(neutral: 80, happy: 5, sad: 5, surprised: 10);
            this.AddEmotionFeedback(neutral: 80, sad: 20);

            this.AddSuggestedScene(3, 5.5);
            this.AddSuggestedScene(7.5, 8.5);
            this.AddSuggestedScene(9.5, 1);
            this.AddSuggestedScene(14.5, 22.43);

            this.AddConsensusScene(10, 1);
        }
    }
}
