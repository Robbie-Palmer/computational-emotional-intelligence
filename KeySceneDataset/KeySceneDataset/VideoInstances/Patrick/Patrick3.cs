/// Patrick3.cs holds the data required to evaluate the video in the dataset
/// known as "Patrick3".
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
    class Patrick3 : VideoResource
    {
        public Patrick3() : base(Dataset.Videos.Patrick3, 35.86)
        {
            this.AddEmotionFeedback(sad: 100);
            this.AddEmotionFeedback(neutral: 75, sad: 25);
            this.AddEmotionFeedback(fearful: 100);
            this.AddEmotionFeedback(neutral: 85, sad: 10, fearful: 5);

            this.AddSuggestedScene(5.5, 1.5);
            this.AddSuggestedScene(10, 2);
            this.AddSuggestedScene(13, 9.5);
            this.AddSuggestedScene(26, 1.5);
            this.AddSuggestedScene(30, 5.86);

            // Note: no scenes with consensus
        }
    }
}
