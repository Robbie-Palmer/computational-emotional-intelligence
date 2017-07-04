/// FeedbackAnalyser.cs provides functions for the analysis of the
/// reliability of feedback provided to act as labels for the dataset.
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
    using KeySceneDataset;
    using System;

    class FeedbackAnalyser
    {
        /// <summary>
        /// Takes the metadata from the video resource and returns the values of metrics which represent the consensus between observations.
        /// </summary>
        /// <param name="video"></param>
        /// <returns></returns>
        public static FeedbackAnalysis GetConsensusMetrics(VideoResource video)
        {
            var keySceneParity = CalcKeySceneParity(video);
            var emotionVariance = CalcEmotionVariance(video.GetEmotionFeedback());

            return new FeedbackAnalysis(video.VideoID, keySceneParity, emotionVariance);
        }

        /// <summary>
        /// Returns the percentage duration of total scene length of consensus scenes over suggested scenes as a calculation of the parity between observers.
        /// High parity means high consensus and therefore reliable data.
        /// </summary>
        /// <param name="video">The input video.</param>
        /// <returns></returns>
        private static double CalcKeySceneParity(VideoResource video)
        {
            var suggScenesDuration = video.GetSuggestedScenesDuration();
            var consenScenesDuration = video.GetConsensusScenesDuration();

            return consenScenesDuration / suggScenesDuration;
        }

        /// <summary>
        /// Returns the Frobernius Norm of the covariance matrix of the values provided by observers for emotional analysis of the video.
        /// A small value means small divergence in opinion.
        /// This method takes an array with n observations with each observation having m dimensions.
        /// The variance and covariances between the components in each observation are calculated and stored in a covariance matrix.
        /// The covariance matrix represents the variance between the observations in multidimensional space.
        /// To be able to compare the variance between different emotion feedback sets, the covariance is normalised, by taking the Frobernius Norm
        /// of this matrix, which equals the square root of the squared sums of the matrix's elements
        /// </summary>
        /// <param name="emotionFeedback">
        ///     Each row of this two dimensional array represents a data entry from an observer, with each column,
        ///     representing the weighting the observer gave to a corresponding emotion display.
        /// </param>
        /// <returns></returns>
        private static double CalcEmotionVariance(double[,] emotionFeedback)
        {
            double[,] covarianceMatrix;
            alglib.covm(emotionFeedback, out covarianceMatrix);

            double sumOfElementSquares = 0;

            for (var i = 0; i < covarianceMatrix.GetLength(0); i++)
            {
                for (var j = 0; j < covarianceMatrix.GetLength(1); j++)
                {
                    sumOfElementSquares += covarianceMatrix[i, j] * covarianceMatrix[i, j];
                }
            }

            return Math.Sqrt(sumOfElementSquares);
        }
        
        public class FeedbackAnalysis
        {
            public Dataset.Videos VideoID { get; }
            public double KeySceneParity { get; }
            public double EmotionVariance { get; }

            public FeedbackAnalysis(Dataset.Videos videoID, double keySceneParity, double emotionVariance)
            {
                VideoID = videoID;
                KeySceneParity = keySceneParity;
                EmotionVariance = emotionVariance;
            }
        }
    }
}
