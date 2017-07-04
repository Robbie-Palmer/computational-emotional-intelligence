/// Rachel2.cs holds the data required to evaluate the video in the dataset
/// known as "Rachel2".
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
    class Rachel2 : VideoResource
    {
        public Rachel2() : base(Dataset.Videos.Rachel2, 16.79)
        {
            this.AddEmotionFeedback(neutral: 100);
            this.AddEmotionFeedback(neutral: 70, happy: 10, sad: 20);
            this.AddEmotionFeedback(neutral: 25, happy: 25, surprised: 25, fearful: 25);

            this.AddSuggestedScene(5, 3.5);
            this.AddSuggestedScene(9, 2);
            this.AddSuggestedScene(13.5, 1.5);
            this.AddSuggestedScene(18.5, 1.5);
            this.AddSuggestedScene(25, 2);
        }
    }
}
