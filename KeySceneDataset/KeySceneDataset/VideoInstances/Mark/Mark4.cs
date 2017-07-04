/// Mark4.cs holds the data required to evaluate the video in the dataset
/// known as "Mark4".
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
    class Mark4 : VideoResource
    {
        public Mark4() : base(Dataset.Videos.Mark4, 48.6)
        {
            this.AddEmotionFeedback(sad: 100 * (1D / 6D), fearful: 100 * (1D / 6D), disgusted: 100 * (2D / 3D));
            this.AddEmotionFeedback(contemptuous: 50, disgusted: 50);
            this.AddEmotionFeedback(contemptuous: 100);
            this.AddEmotionFeedback(contemptuous: 75, disgusted: 25);

            this.AddSuggestedScene(3, 2);
            this.AddSuggestedScene(5.5, 9);
            this.AddSuggestedScene(17, 4.5);
            this.AddSuggestedScene(22.5, 5.5);
            this.AddSuggestedScene(29, 1);
            this.AddSuggestedScene(31.5, 1);
            this.AddSuggestedScene(34, 14.6);

            this.AddConsensusScene(7, 2);
            this.AddConsensusScene(12, 1);
            this.AddConsensusScene(36.5, 3);
            this.AddConsensusScene(43.5, 4.5);
        }
    }
}
