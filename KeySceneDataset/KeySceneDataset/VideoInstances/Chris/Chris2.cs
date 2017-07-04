/// Chris2.cs holds the data required to evaluate the video in the dataset
/// known as "Chris2".
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
    class Chris2 : VideoResource
    {
        public Chris2() : base(Dataset.Videos.Chris2, 16.79)
        {
            this.AddEmotionFeedback(angry: 75, disgusted: 25);
            this.AddEmotionFeedback(angry: 60, contemptuous: 20, disgusted: 20);
            this.AddEmotionFeedback(angry: 20, contemptuous: 80);
            this.AddEmotionFeedback(angry: 80, contemptuous: 10, disgusted: 10);

            this.AddSuggestedScene(0, 3);
            this.AddSuggestedScene(4, 0.5);
            this.AddSuggestedScene(5.5, 1.5);
            this.AddSuggestedScene(7.5, 1.5);
            this.AddSuggestedScene(9.5, 0.5);
            this.AddSuggestedScene(11.5, 1.5);
            this.AddSuggestedScene(13.5, 3.29);
        }
    }
}
