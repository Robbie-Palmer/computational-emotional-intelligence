namespace KeySceneDataset
{
    using System.Collections.Generic;
    using System.Collections.ObjectModel;
    using System.Configuration;

    /// <summary>
    /// This class represents a video from the dataset along with all it's related data such as the ground truth of the evaluation of its contents.
    /// </summary>
    public abstract class VideoResource
    {
        public Dataset.Videos VideoID { get; }

        /// <summary>
        /// The file-path that points to the video input resource.
        /// </summary>
        public string FilePath { get; }

        public double VideoLength { get; }

        public ReadOnlyCollection<VideoFrame> ConsensusScenes { get { return new ReadOnlyCollection<VideoFrame>(consensusScenes); } }

        public ReadOnlyCollection<VideoFrame> SuggestedScenes { get { return new ReadOnlyCollection<VideoFrame>(suggestedScenes); } }

        /// <summary>
        /// A list of the frames which correspond to scenes identified as important by all volunteers that provided feedback for this video.
        /// </summary>
        private IList<VideoFrame> consensusScenes;

        /// <summary>
        /// A list of the frames which correspond to scenes identified as important by at least one volunteer.
        /// </summary>
        private IList<VideoFrame> suggestedScenes;

        /// <summary>
        /// A list of the emotion feedback from observers.
        /// Each emotion feedback instance consists of 8 integer values, which correspond to weightings for each of the 8 possible
        /// emotions displayed throughout the video.
        /// </summary>
        private List<double[]> emotionFeedback;

        protected VideoResource(Dataset.Videos videoId, double videoLength)
        {
            VideoID = videoId;
            VideoLength = videoLength;
            FilePath = ConfigurationManager.AppSettings["DatasetFolder"] + @"\" + videoId + ".mp4";

            consensusScenes = new List<VideoFrame>();
            suggestedScenes = new List<VideoFrame>();
            emotionFeedback = new List<double[]>();
        }

        public double GetConsensusScenesDuration()
        {
            return GetFramesDuration(consensusScenes);
        }
        
        public double GetSuggestedScenesDuration()
        {
            return GetFramesDuration(suggestedScenes);
        }

        public double[,] GetEmotionFeedback()
        {
            double[,] emotionArray = new double[emotionFeedback.Count, emotionFeedback[0].Length];

            for(var i = 0; i < emotionFeedback.Count; i++)
            {
                for(var j = 0; j < emotionFeedback[0].Length; j++)
                {
                    emotionArray[i, j] = emotionFeedback[i][j];
                }
            }

            return emotionArray;
        }

        protected void AddConsensusScene(double startTime, double interval)
        {
            consensusScenes.Add(new VideoFrame(startTime, interval));
        }

        protected void AddSuggestedScene(double startTime, double interval)
        {
            suggestedScenes.Add(new VideoFrame(startTime, interval));
        }

        protected void AddEmotionFeedback(double neutral = 0, double happy = 0, double sad = 0, double angry = 0, double surprised = 0, double fearful = 0, double contemptuous = 0, double disgusted = 0)
        {
            emotionFeedback.Add(new double[] { neutral, happy, sad, angry, surprised, fearful, contemptuous, disgusted });
        }

        private static double GetFramesDuration(IEnumerable<VideoFrame> frames)
        {
            var length = 0.0;

            foreach (var frame in frames)
                length += frame.Interval;

            return length;
        }

        /// <summary>
        /// Represents a frame from the video.
        /// </summary>
        public class VideoFrame
        {
            public double StartTime { get; }
            public double Interval { get; }

            public VideoFrame(double startTime, double interval)
            {
                StartTime = startTime;
                Interval = interval;
            }
        }
    }
}