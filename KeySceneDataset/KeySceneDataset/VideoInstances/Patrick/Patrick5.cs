/// Patrick5.cs holds the data required to evaluate the video in the dataset
/// known as "Patrick5".
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
    class Patrick5 : VideoResource
    {
        public Patrick5() : base(Dataset.Videos.Patrick5, 30.89)
        {
            this.AddEmotionFeedback(neutral: 75, sad: 5, fearful: 10, contemptuous: 10);
            this.AddEmotionFeedback(angry: 100);
            this.AddEmotionFeedback(sad: 100);

            this.AddSuggestedScene(0, 5);
            this.AddSuggestedScene(8, 2);
            this.AddSuggestedScene(13,5);
            this.AddSuggestedScene(19, 11.89);

            // Note: no scenes with consensus
        }
    }
}
