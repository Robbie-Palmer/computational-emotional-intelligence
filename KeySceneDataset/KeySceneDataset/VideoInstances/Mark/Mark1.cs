/// Mark1.cs holds the data required to evaluate the video in the dataset
/// known as "Mark1".
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
    class Mark1 : VideoResource
    {
        public Mark1() : base(Dataset.Videos.Mark1, 30.85)
        {
            this.AddEmotionFeedback(neutral: 70, surprised: 30);
            this.AddEmotionFeedback(neutral: 75, sad: 25);
            this.AddEmotionFeedback(neutral: 100);
            this.AddEmotionFeedback(neutral: 100);

            this.AddSuggestedScene(0, 5);
            this.AddSuggestedScene(8.5, 1.5);
            this.AddSuggestedScene(13.5, 2);
            this.AddSuggestedScene(16.5, 1.5);
            this.AddSuggestedScene(21.5, 3.5);
            this.AddSuggestedScene(28, 2.85);

            this.AddConsensusScene(1.5, 0.5);
            this.AddConsensusScene(8.5, 2);
            this.AddConsensusScene(14.5, 1);
            this.AddConsensusScene(28.5, 1.5);
        }
    }
}
