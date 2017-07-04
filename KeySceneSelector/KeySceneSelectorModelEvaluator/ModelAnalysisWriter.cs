/// ModelAnalysisWriter.cs gathers a list of results to be output,
/// and when called upon writes the results to a text file.
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
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using System.Linq;

    class ModelAnalysisWriter
    {
        private string filePath;
        private IList<ModelAnalysisOutput> outputs;

        public ModelAnalysisWriter(string fileName)
        {
            var outputFolder = ConfigurationManager.AppSettings["ModelAnalysisOutput"];
            filePath = outputFolder + @"\" + fileName;

            outputs = new List<ModelAnalysisOutput>();
        }

        public void AddModelAnalysisOutput(string modelName, double modelSetting, ModelEvaluator.ModelAnalysis metrics)
        {
            outputs.Add(new ModelAnalysisOutput(modelName, modelSetting, metrics));
        }

        public void OutputModelAnalysis()
        {
            this.outputs = this.outputs.OrderBy(x => x.ModelName)
                    .ThenBy(y => y.ModelSetting).ToList();

            var directoryPath = Path.GetDirectoryName(filePath);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            using (var file = new StreamWriter(filePath))
            {
                foreach (var output in outputs)
                {
                    var metrics = output.Metrics;
                    file.WriteLine("{0, -20}-{1: 0.00}:" +
                        "\tConsensus Match Increase = {2: 0.00}%" +
                        "\tFalse Positive Reduction = {3: 0.00}%" +
                        "\tPerc of Total Video = {4: 0.00}%",
                        output.ModelName, output.ModelSetting,
                        metrics.ConsensusMatchIncrease * 100,
                        metrics.FalsePositiveReduction * 100,
                        metrics.PercentOfVideoChosen * 100);
                }
            }
        }
        
        private class ModelAnalysisOutput
        {
            public string ModelName { get; }
            public double ModelSetting { get; }
            public ModelEvaluator.ModelAnalysis Metrics { get; }

            public ModelAnalysisOutput(string modelName, double modelSetting, ModelEvaluator.ModelAnalysis metrics)
            {
                ModelName = modelName;
                ModelSetting = modelSetting;
                Metrics = metrics;
            }
        }
    }
}
