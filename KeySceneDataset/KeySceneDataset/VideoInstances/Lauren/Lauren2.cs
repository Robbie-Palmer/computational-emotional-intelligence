/// Lauren2.cs holds the data required to evaluate the video in the dataset
/// known as "Lauren2".
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
    class Lauren2 : VideoResource
    {
        public Lauren2() : base(Dataset.Videos.Lauren2, 16.79)
        {
            this.AddEmotionFeedback(sad: 50, fearful: 50);
            this.AddEmotionFeedback(neutral: 80, contemptuous: 20);
            this.AddEmotionFeedback(neutral: 80, sad: 10, contemptuous: 10);

            this.AddSuggestedScene(0, 2);
            this.AddSuggestedScene(17, 1);
        }
    }
}
