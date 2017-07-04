/// Program.cs is responsible for executing the workflow to iterate
/// through the dataset, as made available by the KeySceneDataset,
/// and for each video calculate metrics to establish the reliabilty
/// of the associated feedback for acting as ground truth labels of the
/// state of the content of each video.
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

namespace DatasetEvaluator
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using KeySceneDataset;
    using System.IO;
    using static FeedbackAnalyser;
    using System.Configuration;

    class Program
    {
        const string EVALUATION_FOLDER_KEY = "EvaluationOutput";

        static void Main(string[] args)
        {
            var videoIDs = (Dataset.Videos[]) Enum.GetValues(typeof(Dataset.Videos));

            var feedbackAnalysis = new List<FeedbackAnalysis>();

            foreach(var uniqueID in videoIDs)
            {
                var video = Dataset.GetVideoResource(uniqueID);
                var consensusMetrics = GetConsensusMetrics(video);
                feedbackAnalysis.Add(consensusMetrics);
            }

            var feedbackByDescParity = feedbackAnalysis.OrderByDescending(x => x.KeySceneParity);
            var feedbackByVariance = feedbackAnalysis.OrderBy(x => x.EmotionVariance);

            var outputFolder = ConfigurationManager.AppSettings[EVALUATION_FOLDER_KEY];

            Console.WriteLine("Feedback ordered in decreasing order by key scene parity: ");

            OutputFeedback(outputFolder + @"\" + "FeedBackOrderedByDescParity.txt", feedbackByDescParity);

            Console.WriteLine();
            
            Console.WriteLine("Feedback ordered by emotional variance: ");
            
            OutputFeedback(outputFolder + @"\" + "FeedBackOrderedByVariance.txt", feedbackByVariance);
        }

        private static void OutputFeedback(string filePath, IOrderedEnumerable<FeedbackAnalysis> feedbackEntries)
        {
            using (var file = new StreamWriter(filePath))
            {
                foreach (var feedback in feedbackEntries)
                {
                    file.WriteLine("{0}\tParity = {1}\tVariance = {2}", feedback.VideoID, feedback.KeySceneParity, feedback.EmotionVariance);
                }
            }
        }
    }
}
