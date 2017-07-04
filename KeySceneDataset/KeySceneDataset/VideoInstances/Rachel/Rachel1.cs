/// Rachel1.cs holds the data required to evaluate the video in the dataset
/// known as "Rachel1".
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
    class Rachel1 : VideoResource
    {
        public Rachel1() : base(Dataset.Videos.Rachel1, 20.36)
        {
            this.AddEmotionFeedback(angry: 75, fearful: 25);
            this.AddEmotionFeedback(neutral: 100 * (1D / 3D), sad: 100 * (1D / 3D), fearful: 100 * (1D / 3D));
            this.AddEmotionFeedback(sad: 25, angry: 75);
            this.AddEmotionFeedback(neutral: 15, sad: 15, contemptuous: 15, disgusted: 55);

            this.AddSuggestedScene(0, 4);
            this.AddSuggestedScene(8, 1);
            this.AddSuggestedScene(11, 13);
            this.AddSuggestedScene(14, 2);
        }
    }
}
