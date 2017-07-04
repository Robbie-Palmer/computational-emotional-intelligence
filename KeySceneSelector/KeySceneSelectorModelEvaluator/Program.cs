/// Program.cs is responsible for executing the workflow to create and
/// evaluate key scene selector models against the labels in the dataset
/// and output the results.
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
    using System.IO;
    using static ModelEvaluator;

    class Program
    {
        private enum KSS { StateTransition, StrengthTransition, PeakEmotion };

        public static void Main(string[] args)
        {
            var dataset = Dataset.GetSetOfReliableVideos();

            var stateChangeThresholds = EmotionStateTransitionMarkovChain.GetSetOfTransitionLikelihoods();

            // One by one wait for evaluation to complete, then use each model to get the identified key scenes
            foreach (var video in dataset)
            {
                var vidID = video.VideoID;

                var strengthModel = new EmotionStrengthChangePredictor(video.FilePath);
                var peakModel = new PeakEmotionDetector(video.FilePath);
                var stateModel = new EmotionStateTransitionMarkovChain(video.FilePath);

                // Since using same video for all models, last valid timestamp same for all, as share evaluated scenes
                var modelEvaluator = new ModelEvaluator(video, strengthModel.GetLastValidTimestamp());

                EvaluateModelWithVaryingThreshold(modelEvaluator, 0.01, 0.4, 0.01, peakModel, vidID, "Peak-Emotion-ModelAnalysis.txt");
                EvaluateModelWithVaryingThreshold(modelEvaluator, 0.08, 0.54, 0.01, strengthModel, vidID, "Strength-Change-ModelAnalysis.txt");
                
                var writer = new ModelAnalysisWriter(vidID + @"\" + "State-Change-ModelAnalysis.txt");
                foreach (var threshold in stateChangeThresholds)
                {
                    var keyScenes = stateModel.GetKeyScenes(threshold);
                    var modelAnalysis = modelEvaluator.AnalyseResults(keyScenes);
                    writer.AddModelAnalysisOutput(string.Empty, threshold, modelAnalysis);
                }

                writer.OutputModelAnalysis();
            }

            var analysisWriter = new ModelAnalysisWriter("Model-Averages.txt");
            
            AddAverageAnalysis(KSS.PeakEmotion, "Peak Model ", 0.01, 0.4, 0.01, analysisWriter, dataset);
            AddAverageAnalysis(KSS.StrengthTransition, "Strength Model ", 0.08, 0.54, 0.01, analysisWriter, dataset);

            foreach (var threshold in stateChangeThresholds)
            {
                var results = GetResultsForThreshold(KSS.StateTransition, threshold, dataset);
                analysisWriter.AddModelAnalysisOutput("State Model " + results.Count, threshold, GetAverageAnalysis(results));
            }

            analysisWriter.OutputModelAnalysis();
        }

        private static void AddAverageAnalysis(
            KSS keySceneSelector,
            string fileName,
            double startThresh,
            double endThresh,
            double increment,
            ModelAnalysisWriter writer,
            ISet<VideoResource> dataset)
        {
            for (var i = startThresh; i <= endThresh; i += increment)
            {
                var results = GetResultsForThreshold(keySceneSelector, i, dataset);
                writer.AddModelAnalysisOutput(fileName + results.Count, i, GetAverageAnalysis(results));
            }
        }

        private static IList<ModelAnalysis> GetResultsForThreshold(KSS keySceneSelector, double thresh, ISet<VideoResource> dataset)
        {
            var results = new List<ModelAnalysis>();

            foreach (var video in dataset)
            {
                var strengthModel = new EmotionStrengthChangePredictor(video.FilePath);

                // Since using same video for all models, last valid timestamp same for all, as share evaluated scenes
                var modelEvaluator = new ModelEvaluator(video, strengthModel.GetLastValidTimestamp());

                IEnumerable<Scene> keyScenes = null;

                switch (keySceneSelector)
                {
                    case KSS.StrengthTransition: keyScenes = strengthModel.GetKeyScenes(thresh); break;
                    case KSS.PeakEmotion: keyScenes = new PeakEmotionDetector(video.FilePath).GetKeyScenes(thresh); break;
                    case KSS.StateTransition: keyScenes = new EmotionStateTransitionMarkovChain(video.FilePath).GetKeyScenes(thresh); break;
                }

                var result = modelEvaluator.AnalyseResults(keyScenes);
                if (!double.IsNaN(result.ConsensusMatchIncrease) && !double.IsNaN(result.FalsePositiveReduction) && result.PercentOfVideoChosen <= 0.5)
                    results.Add(result);
            }

            return results;
        }

        private static ModelAnalysis GetAverageAnalysis(IList<ModelAnalysis> analysis)
        {
            var consensusSum = 0.0;
            var falsePosSum = 0.0;
            var percVideoSum = 0.0;

            foreach (var result in analysis)
            {
                consensusSum += result.ConsensusMatchIncrease;
                falsePosSum += result.FalsePositiveReduction;
                percVideoSum += result.PercentOfVideoChosen;
            }

            var avgConsensusMatch = consensusSum / analysis.Count;
            var avgFalsePosReduction = falsePosSum / analysis.Count;
            var avgPercVideoChosen = percVideoSum / analysis.Count;

            return new ModelAnalysis(avgConsensusMatch, avgFalsePosReduction, avgPercVideoChosen);
        }

        private static void EvaluateModelWithVaryingThreshold(
            ModelEvaluator evaluator,
            double startThresh,
            double endThresh,
            double increment,
            KeySceneSelector kss,
            Dataset.Videos vidID,
            string fileName)
        {
            var analysisWriter = new ModelAnalysisWriter(vidID + @"\" + fileName);
            for (var i = startThresh; i <= endThresh; i += increment)
            {
                var keyScenes = kss.GetKeyScenes(i);
                var modelAnalysis = evaluator.AnalyseResults(keyScenes);
                analysisWriter.AddModelAnalysisOutput(string.Empty, i, modelAnalysis);
            }

            analysisWriter.OutputModelAnalysis();
        }

        private static void OutputKeyScenes(string filePath, IEnumerable<Scene> kssScenes)
        {
            var directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using (var file = new StreamWriter(filePath))
            {
                foreach (var scene in kssScenes)
                {
                    file.WriteLine("Start-time: {0}\tEndTime: {1}", scene.StartTime, scene.EndTime);
                }
            }
        }
    }
}
