/// Lauren1.cs holds the data required to evaluate the video in the dataset
/// known as "Lauren1".
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
    class Lauren1 : VideoResource
    {
        public Lauren1() : base(Dataset.Videos.Lauren1, 20.36)
        {
            this.AddEmotionFeedback(sad: 100);
            this.AddEmotionFeedback(neutral: 75, sad: 10, fearful: 15);
            this.AddEmotionFeedback(neutral: 70, sad: 30);
            this.AddEmotionFeedback(sad: 50, surprised: 50);

            this.AddSuggestedScene(0, 3);
            this.AddSuggestedScene(4.5, 1);
            this.AddSuggestedScene(7, 2.5);
            this.AddSuggestedScene(11.5, 3);
        }
    }
}
