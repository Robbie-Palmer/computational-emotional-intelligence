/// EmotionStrengthChangePredictor.cs inherits from KeySceneSelector.cs
/// and implements the GetKeyFrames method, to return frames which
/// correspond with unlikely emotion intensity transitions.
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
    using static CognitiveServices.EmotionDetectionClient;
    using static CognitiveServices.EmotionStrengthDetector;

    public class EmotionStrengthChangePredictor : KeySceneSelector
    {
        public EmotionStrengthChangePredictor(string filePath) : base(filePath)
        {
        }
        
        protected override IList<EmotionFrame> GetKeyFrames(List<EmotionFrame> allScenes, double threshold)
        {
            return UnlikelyEventModel.GetUnlikelyScenes(allScenes, threshold, CalcLikelihood);
        }

        private static double CalcLikelihood(EmotionFrame currentFrame, EmotionFrame nextFrame)
        {
            var oldScore = ApplySentimentFilter(currentFrame.Emotion, currentFrame.EmotionStrength);
            var newScore = ApplySentimentFilter(nextFrame.Emotion, nextFrame.EmotionStrength);

            return CalcLikelihood(oldScore, newScore);
        }

        private static double CalcLikelihood(double oldValue, double newValue)
        {
            double sigma = 0.5;
            double a = -1;
            double b = 1;

            double pdf = TruncatedNormalDistributionProbabilityCalculator.truncated_normal_ab_pdf(newValue, oldValue, sigma, a, b);
            double maxPdf = TruncatedNormalDistributionProbabilityCalculator.truncated_normal_ab_pdf(b, b, sigma, a, b);

            // Scale the pdf to represent probability without integrating
            double scaledPdf = pdf / maxPdf;

            return scaledPdf;
        }

        private static double ApplySentimentFilter(Emotion emotion, double emotionalScore)
        {
            if (emotion == Emotion.Neutrality)
                return 0;

            // If isn't negative keep as is
            if (NotNegativeOrNeutral(emotion))
                return emotionalScore;

            // If negative emotion, make it a negative value
            return 0 - emotionalScore;
        }

        private static bool NotNegativeOrNeutral(Emotion emotion)
        {
            return emotion == Emotion.Happiness || emotion == Emotion.Surprise;
        }

        /// <summary>
        /// This class provides functions to calculate the probability distribution function of a truncated normal distribution.
        /// It is adapted from the work of John Burkardt, and translated from C++ to C# for the purposes of this project.
        /// Licensing: This code is distributed under the GNU LGPL license.
        /// </summary>
        private class TruncatedNormalDistributionProbabilityCalculator
        {
            /// <summary>
            ///  Purpose: TRUNCATED_NORMAL_AB_PDF evaluates the truncated Normal PDF.
            ///  Licensing: This code is distributed under the GNU LGPL license.
            ///  Modified:  April 2017 and August 2013
            ///  Author: John Burkardt
            ///  Translated from C++ to C# by Robert Palmer in April 2017
            /// </summary>
            /// <param name="x">The argument of the PDF</param>
            /// <param name="mu">The mean of the parent Normal distribution</param>
            /// <param name="sigma">The standard deviation of the parent Normal distribution</param>
            /// <param name="a">The lower truncation limit</param>
            /// <param name="b">The upper truncation limit</param>
            /// <returns>The value of the PDF</returns>
            public static double truncated_normal_ab_pdf(double x, double mu, double sigma, double a, double b)
            {
                double alpha = (a - mu) / sigma;
                double beta = (b - mu) / sigma;
                double xi = (x - mu) / sigma;

                double alpha_cdf = normal_01_cdf(alpha);
                double beta_cdf = normal_01_cdf(beta);
                double xi_pdf = normal_01_pdf(xi);

                double pdf = xi_pdf / (beta_cdf - alpha_cdf) / sigma;

                return pdf;
            }

            /// <summary>
            ///  Purpose: NORMAL_01_CDF evaluates the Normal 01 CDF.
            ///  Licensing: This code is distributed under the GNU LGPL license.
            ///  Modified:  April 2017 and 10 February 1999
            ///  Author: John Burkardt
            ///  Translated to C# by Robert Palmer in April 2017
            ///  
            ///  Reference:
            ///  A G Adams,
            ///  Areas Under the Normal Curve,
            ///  Algorithm 39,
            ///  Computer j.,
            ///  Volume 12, pages 197-198, 1969.
            /// </summary>
            /// <param name="x">The argument of the CDF</param>
            /// <returns>The value of the CDF</returns>
            private static double normal_01_cdf(double x)
            {
                double q = CalcQ(x);

                //  Take account of sign of X.

                double cdf = q;

                if (x >= 0.0)
                {
                    cdf = 1.0 - cdf;
                }

                return cdf;
            }


            /// <summary>
            ///  Licensing: This code is distributed under the GNU LGPL license.
            ///  Modified:  April 2017 and 10 February 1999
            ///  Author: John Burkardt
            ///  Translated to C# by Robert Palmer in April 2017
            ///  
            ///  Reference:
            ///  A G Adams,
            ///  Areas Under the Normal Curve,
            ///  Algorithm 39,
            ///  Computer j.,
            ///  Volume 12, pages 197-198, 1969.
            /// </summary>
            /// <param name="x">The argument of the CDF</param>
            /// <returns>The value of Q to aid in calculating the CDF</returns>
            private static double CalcQ(double x)
            {
                if (Math.Abs(x) > 12.7)
                    return 0.0;

                double y = 0.5 * x * x;

                if (Math.Abs(x) <= 1.28)
                {
                    double a1 = 0.398942280444;
                    double a2 = 0.399903438504;
                    double a3 = 5.75885480458;
                    double a4 = 29.8213557808;
                    double a5 = 2.62433121679;
                    double a6 = 48.6959930692;
                    double a7 = 5.92885724438;

                    return 0.5 - Math.Abs(x) * (a1 - a2 * y / (y + a3 - a4 / (y + a5 + a6 / (y + a7))));
                }

                double b0 = 0.398942280385;
                double b1 = 3.8052E-08;
                double b2 = 1.00000615302;
                double b3 = 3.98064794E-04;
                double b4 = 1.98615381364;
                double b5 = 0.151679116635;
                double b6 = 5.29330324926;
                double b7 = 4.8385912808;
                double b8 = 15.1508972451;
                double b9 = 0.742380924027;
                double b10 = 30.789933034;
                double b11 = 3.99019417011;

                return Math.Exp(-y) * b0 / (Math.Abs(x) - b1
                      + b2 / (Math.Abs(x) + b3
                      + b4 / (Math.Abs(x) - b5
                      + b6 / (Math.Abs(x) + b7
                      - b8 / (Math.Abs(x) + b9
                      + b10 / (Math.Abs(x) + b11))))));
            }

            /// <summary>
            ///  Purpose: NORMAL_01_PDF evaluates the Normal 01 PDF.
            ///  Licensing: This code is distributed under the GNU LGPL license.
            ///  Modified:  April 2017 and 18 September 2004
            ///  Author: John Burkardt
            ///  Translated to C# by Robert Palmer in April 2017
            ///  
            ///  Discussion:
            ///  The Normal 01 PDF is also called the "Standard Normal" PDF, or
            ///  the Normal PDF with 0 mean and standard deviation 1.
            ///  PDF(X) = exp ( - 0.5 * X^2 ) / sqrt ( 2 * PI )
            /// </summary>
            /// <param name="x">The argument of the PDF</param>
            /// <returns>The value of the PDF</returns>
            private static double normal_01_pdf(double x)
            {
                const double r8_pi = 3.14159265358979323;

                return Math.Exp(-0.5 * x * x) / Math.Sqrt(2.0 * r8_pi);
            }
        }
    }
}
