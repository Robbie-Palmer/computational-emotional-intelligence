/// Patrick2.cs holds the data required to evaluate the video in the dataset
/// known as "Patrick2".
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
    class Patrick2 : VideoResource
    {
        public Patrick2() : base(Dataset.Videos.Patrick2, 39.55)
        {
            this.AddEmotionFeedback(happy: 100);
            this.AddEmotionFeedback(happy: 100);
            this.AddEmotionFeedback(happy: 100);
            this.AddEmotionFeedback(neutral: 70, happy: 30);

            this.AddSuggestedScene(3.5, 5);
            this.AddSuggestedScene(10.5, 5);
            this.AddSuggestedScene(17, 5.5);
            this.AddSuggestedScene(30, 9.55);

            this.AddConsensusScene(32.5, 1.5);
        }
    }
}
