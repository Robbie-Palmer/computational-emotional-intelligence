/// Patrick4.cs holds the data required to evaluate the video in the dataset
/// known as "Patrick4".
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
    class Patrick4 : VideoResource
    {
        public Patrick4() : base(Dataset.Videos.Patrick4, 37.99)
        {
            this.AddEmotionFeedback(angry: 100);
            this.AddEmotionFeedback(neutral: 15, angry: 15, contemptuous: 70);
            this.AddEmotionFeedback(angry: 30, contemptuous: 70);
            this.AddEmotionFeedback(angry: 75, disgusted: 25);

            this.AddSuggestedScene(6, 5);
            this.AddSuggestedScene(11.5, 10.5);
            this.AddSuggestedScene(26.5, 2.5);
            this.AddSuggestedScene(31.5, 6.44);

            this.AddConsensusScene(34, 3.99);
        }
    }
}
