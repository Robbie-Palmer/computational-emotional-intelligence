/// Dataset.cs provides a public interface to assess and access the videos
/// in the dataset, and the associated metadata.
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

namespace KeySceneDataset
{
    using System.Collections.Generic;
    using VideoInstances;

    public class Dataset
    {
        public enum Videos
        {
            Mark1, Mark2, Mark3, Mark4, Mark5, Mark6,
            Patrick1, Patrick2, Patrick3, Patrick4, Patrick5, Patrick6,
            Chris1, Chris2,
            Lauren1, Lauren2,
            Rachel1, Rachel2
        };

        private static IEnumerable<Videos> reliableVideoIDs = new HashSet<Videos>
                           { Videos.Mark1, Videos.Mark2, Videos.Mark3, Videos.Mark4, Videos.Patrick1, Videos.Patrick4, Videos.Patrick6 }; 

        public static VideoResource GetVideoResource(Videos inputVideo)
        {
            switch(inputVideo)
            {
                case Videos.Mark1:
                    return new Mark1();
                case Videos.Mark2:
                    return new Mark2();
                case Videos.Mark3:
                    return new Mark3();
                case Videos.Mark4:
                    return new Mark4();
                case Videos.Mark5:
                    return new Mark5();
                case Videos.Mark6:
                    return new Mark6();

                case Videos.Patrick1:
                    return new Patrick1();
                case Videos.Patrick2:
                    return new Patrick2();
                case Videos.Patrick3:
                    return new Patrick3();
                case Videos.Patrick4:
                    return new Patrick4();
                case Videos.Patrick5:
                    return new Patrick5();
                case Videos.Patrick6:
                    return new Patrick6();

                case Videos.Chris1:
                    return new Chris1();
                case Videos.Chris2:
                    return new Chris2();

                case Videos.Lauren1:
                    return new Lauren1();
                case Videos.Lauren2:
                    return new Lauren2();

                case Videos.Rachel1:
                    return new Rachel1();
                case Videos.Rachel2:
                    return new Rachel2();

                default:
                    return null;
            }
        }

        public static ISet<VideoResource> GetSetOfReliableVideos()
        {
            var videos = new HashSet<VideoResource>();

            foreach (var vidID in reliableVideoIDs)
                videos.Add(Dataset.GetVideoResource(vidID));

            return videos;
        }
    }
}
