/// SceneManipulator.cs contains a number of functions to alter scenes,
/// produce scenes from frames, merge scenes etc.
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
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using static CognitiveServices.EmotionStrengthDetector;

    public class SceneManipulator
    {
        public static IEnumerable<Scene> EmotionFramesToScenes(IList<EmotionFrame> frames)
        {
            var scenes = MergeFramesIntoScenes(frames);
            RoundSceneTimesToHalfSeconds(scenes);
            var mergedScenes = MergeScenes(scenes, false);

            return mergedScenes;
        }

        public static Scene EmotionFrameToScene(EmotionFrame frame)
        {
            var scene = new Scene(frame.StartTime, frame.StartTime + frame.Interval);
            RoundSceneTimesToHalfSeconds(scene);

            return scene;
        }

        public static IEnumerable<Scene> MergeScenes(IEnumerable<Scene> allResults, bool deepCopy)
        {
            // Removes duplicates, then for each start time selects the entry with the latest end time, and for each end time selects the entry with the
            // earliest start time, and orders by start-time, end-time
            var overlappedScenes = allResults.Distinct()
                .GroupBy(a => a.StartTime).Select(b => b.OrderBy(c => c.StartTime).ThenByDescending(d => d.EndTime).FirstOrDefault())
                .GroupBy(a => a.EndTime).Select(b => b.OrderByDescending(c => c.EndTime).ThenBy(d => d.StartTime).FirstOrDefault())
                .OrderBy(e => e.StartTime).ThenByDescending(f => f.EndTime).ToList();

            // Make a deep copy
            if(deepCopy)
            {
                var copy = new List<Scene>();

                foreach (var scene in overlappedScenes)
                    copy.Add(new Scene(scene.StartTime, scene.EndTime));

                overlappedScenes = copy;
            }

            for (var i = 0; i < overlappedScenes.Count - 1; i++)
            {
                var scene = overlappedScenes[i];

                for (var j = i + 1; j < overlappedScenes.Count; j++)
                {
                    var otherScene = overlappedScenes[j];

                    // Passed point of comparison as are ordered
                    if (otherScene.StartTime > scene.EndTime)
                        break;

                    // Can't merge scenes that don't overlap or touch
                    if (otherScene.Equals(scene) || !scene.OverlapsOrTouches(otherScene))
                        continue;

                    // Because ordered, the min start time will be start time of scene
                    // and end time will be the max end time of the two scenes, so update
                    // scene to represent the two scenes merged
                    scene.EndTime = Math.Max(scene.EndTime, otherScene.EndTime);

                    // Remove the scene at j, and reevaluate the updated scene at i
                    overlappedScenes.RemoveAt(j);
                    i--;
                    break;
                }
            }

            return overlappedScenes;
        }

        private static IList<Scene> MergeFramesIntoScenes(IList<EmotionFrame> frames)
        {
            var scenes = new List<Scene>();

            int i = 0;
            while (i < frames.Count)
            {
                var frame = frames[i];

                var endTime = frame.StartTime + frame.Interval;

                // Merge following scenes with this scene, if all in immediate sequence
                int j = i + 1;
                while (j < frames.Count && frames[j].StartTime == endTime)
                {
                    endTime += frames[j].Interval;
                    j++;
                }

                // Set i to equal the next unmerged scene
                i = j;

                scenes.Add(new Scene(frame.StartTime, endTime));
            }

            return scenes;
        }

        private static void RoundSceneTimesToHalfSeconds(IEnumerable<Scene> scenes)
        {
            foreach (var scene in scenes)
            {
                RoundSceneTimesToHalfSeconds(scene);
            }
        }

        private static void RoundSceneTimesToHalfSeconds(Scene scene)
        {
            var startTimeInSeconds = scene.StartTime / 1000000;
            var endTimeInSeconds = scene.EndTime / 1000000;

            var startTimeInteger = Math.Truncate(startTimeInSeconds);
            var startTimeFraction = startTimeInSeconds - startTimeInteger;

            // Round start time down to previous half second mark
            if (startTimeFraction >= 0.5)
                scene.StartTime = startTimeInteger + 0.5;
            else
                scene.StartTime = startTimeInteger;


            var endTimeInteger = Math.Truncate(endTimeInSeconds);
            var endTimeFraction = endTimeInSeconds - endTimeInteger;

            // Round end time up to next half second mark
            if (endTimeFraction <= 0.5)
                scene.EndTime = endTimeInteger + 0.5;
            else
                scene.EndTime = endTimeInteger + 1;
        }
    }
}
