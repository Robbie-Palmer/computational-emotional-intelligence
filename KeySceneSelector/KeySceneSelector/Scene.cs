/// Scene.cs represents a time period in a video, which can overlap or
/// touch other scenes temporally.
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

namespace KeySceneSelector
{
    public class Scene
    {
        public double StartTime { get; set; }
        public double EndTime { get; set; }

        public Scene(double startTime, double endTime)
        {
            StartTime = startTime;
            EndTime = endTime;
        }

        public bool OverlapsOrTouches(Scene otherScene)
        {
            return this.StartTime <= otherScene.EndTime && this.EndTime >= otherScene.StartTime;
        }

        public bool Overlaps(Scene otherScene)
        {
            return this.StartTime < otherScene.EndTime && this.EndTime > otherScene.StartTime;
        }

        public override bool Equals(object obj)
        {
            var otherScene = obj as Scene;
            return otherScene != null && this.StartTime == otherScene.StartTime && this.EndTime == otherScene.EndTime;
        }

        public override int GetHashCode()
        {
            return new { StartTime, EndTime }.GetHashCode();
        }
    }
}
