/// ModelEvaluator.cs is responsible for comparing the results from a
/// KeySceneSelector implementation against the labels associated with
/// a video in the dataset. It calculates metrics to evaluate success
/// and returns the results.
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

namespace KeySceneSelectorModelEvaluator
{
    using KeySceneDataset;
    using KeySceneSelector;
    using System.Collections.Generic;
    using static KeySceneDataset.VideoResource;

    class ModelEvaluator
    {
        private IEnumerable<VideoFrame> consensusScenes;
        private IEnumerable<VideoFrame> suggestedScenes;
        private double videoLength;

        public ModelEvaluator(VideoResource video, double lastValidTimestamp)
        {
            consensusScenes = CropFrames(video.ConsensusScenes, lastValidTimestamp);
            suggestedScenes = CropFrames(video.SuggestedScenes, lastValidTimestamp);
            videoLength = lastValidTimestamp;
        }

        public ModelAnalysis AnalyseResults(IEnumerable<Scene> kssKeyScenes)
        {
            var kssScenesDuration = GetScenesDuration(kssKeyScenes);

            var percentOfVideoChosen = kssScenesDuration / videoLength;

            var consensusMatch = GetConsensusMatch(kssKeyScenes);
            var falsePosMatch = GetFalsePositiveMatch(kssKeyScenes, kssScenesDuration);

            var consMatchPercIncrease = (consensusMatch / percentOfVideoChosen) - 1.0;
            var falsePositiveReduction = 0 - ((falsePosMatch / percentOfVideoChosen) - 1.0);

            return new ModelAnalysis(consMatchPercIncrease, falsePositiveReduction, percentOfVideoChosen);
        }

        private double GetConsensusMatch(IEnumerable<Scene> kssKeyScenes)
        {
            var overlapScenesDuration = GetScenesDuration(GetOverlappedSections(kssKeyScenes, consensusScenes));
            var datasetScenesDuration = GetScenesDuration(consensusScenes);

            return overlapScenesDuration / datasetScenesDuration;
        }

        private double GetFalsePositiveMatch(IEnumerable<Scene> kssKeyScenes, double kssScenesDuration)
        {
            var suggScenesDuration = GetScenesDuration(suggestedScenes);
            var overlapScenesDuration = GetScenesDuration(GetOverlappedSections(kssKeyScenes, suggestedScenes));

            // Calculate the length of identified scenes that do not match
            var kssUnmarkedLength = kssScenesDuration - overlapScenesDuration;
            var totalUnmarkedLength = videoLength - suggScenesDuration;

            // Return percent of unmarked scenes chosen
            return kssUnmarkedLength / totalUnmarkedLength;
        }

        private static IEnumerable<VideoFrame> GetOverlappedSections(IEnumerable<Scene> kssKeyScenes, IEnumerable<VideoFrame> datasetScenes)
        {
            var overlapScenes = new List<VideoFrame>();

            foreach (var datasetScene in datasetScenes)
            {
                var datasetSceneEndTime = datasetScene.StartTime + datasetScene.Interval;

                foreach (var kssScene in kssKeyScenes)
                {
                    if (!kssScene.Overlaps(new Scene(datasetScene.StartTime, datasetSceneEndTime)))
                        continue;

                    var overlapStartTime = kssScene.StartTime >= datasetScene.StartTime ? kssScene.StartTime : datasetScene.StartTime;
                    var overlapEndTime = kssScene.EndTime <= datasetSceneEndTime ? kssScene.EndTime : datasetSceneEndTime;

                    overlapScenes.Add(new VideoFrame(overlapStartTime, overlapEndTime - overlapStartTime));
                }
            }

            return overlapScenes;
        }

        private static double GetScenesDuration(IEnumerable<VideoFrame> scenes)
        {
            var duration = 0.0;
            foreach (var scene in scenes)
                duration += scene.Interval;

            return duration;
        }

        private static double GetScenesDuration(IEnumerable<Scene> scenes)
        {
            var duration = 0.0;
            foreach (var scene in scenes)
                duration += scene.EndTime - scene.StartTime;

            return duration;
        }

        private static IEnumerable<VideoFrame> CropFrames(IReadOnlyCollection<VideoFrame> frames, double newEndTime)
        {
            var croppedFrames = new List<VideoFrame>();
            foreach (var frame in frames)
            {
                // Assume ordered list
                if (frame.StartTime >= newEndTime)
                    break;
                
                if ((frame.StartTime + frame.Interval) < newEndTime)
                    croppedFrames.Add(frame);
                else
                    croppedFrames.Add(new VideoFrame(frame.StartTime, newEndTime - frame.StartTime));
            }

            return croppedFrames;
        }

        public class ModelAnalysis
        {
            public double ConsensusMatchIncrease { get; set; }
            public double FalsePositiveReduction { get; set; }
            public double PercentOfVideoChosen { get; }

            public ModelAnalysis(double consensusMatchIncrease, double falsePositiveReduction, double percentOfVideoChosen)
            {
                ConsensusMatchIncrease = consensusMatchIncrease;
                FalsePositiveReduction = falsePositiveReduction;
                PercentOfVideoChosen = percentOfVideoChosen;
            }
        }
    }
}
