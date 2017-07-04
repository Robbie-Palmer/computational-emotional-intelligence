/// KeySceneSelector.cs is an abstract class which represents the base of
/// a statistical model that will analyse a video and determine which
/// scenes it deems to be the most important. This class is responsible
/// for contacting the appropriate API to retrieve emotion data, and
/// for caching and retrieving past results. It also defines the public
/// interface for retrieving the key scenes.
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
    using CognitiveServices;
    using Newtonsoft.Json;
    using System.Collections.Generic;
    using System.Configuration;
    using System.IO;
    using static CognitiveServices.EmotionStrengthDetector;

    public abstract class KeySceneSelector
    {
        protected static Dictionary<string, List<EmotionFrame>> EvaluatedFramesCacheLocal;

        private string filePath;
        private string cacheFilePath;

        protected KeySceneSelector(string filePath)
        {
            this.filePath = filePath;

            var outputFolder = ConfigurationManager.AppSettings["AllScenesOutput"];
            var fileName = Path.GetFileNameWithoutExtension(filePath) + ".txt";

            cacheFilePath = outputFolder + @"\" + fileName;

            EvaluatedFramesCacheLocal = new Dictionary<string, List<EmotionFrame>>();
        }

        public IEnumerable<Scene> GetKeyScenes(double threshold)
        {
            var allScenes = GetEvaluatedFrames();
            var emotionFrames = GetKeyFrames(allScenes, threshold);
            return SceneManipulator.EmotionFramesToScenes(emotionFrames);
        }

        protected abstract IList<EmotionFrame> GetKeyFrames(List<EmotionFrame> allScenes, double threshold);

        public double GetLastValidTimestamp()
        {
            var allFrames = GetEvaluatedFrames();
            var lastFrame = allFrames[allFrames.Count - 1];
            var lastScene = SceneManipulator.EmotionFrameToScene(lastFrame);

            return lastScene.EndTime;
        }

        private List<EmotionFrame> GetEvaluatedFrames()
        {
            List<EmotionFrame> evaluatedFrames;
            if (EvaluatedFramesCacheLocal.TryGetValue(filePath, out evaluatedFrames))
                return evaluatedFrames;
            
            if(TryGetEvaluatedFramesFromCacheFile(out evaluatedFrames))
            {
                EvaluatedFramesCacheLocal[filePath] = evaluatedFrames;
                return evaluatedFrames;
            }

            var evalFramesTask = new EmotionStrengthDetector().GetFrameEmotionStrengthAndTime(filePath);
            evalFramesTask.Wait();

            evaluatedFrames = evalFramesTask.Result;
            EvaluatedFramesCacheLocal[filePath] = evaluatedFrames;
            StoreEvaluatedFramesInCacheFile(evaluatedFrames);

            return evaluatedFrames;
        }

        private bool TryGetEvaluatedFramesFromCacheFile(out List<EmotionFrame> evaluatedFrames)
        {
            evaluatedFrames = null;

            if (!File.Exists(cacheFilePath))
                return false;

            var serialisedResult = File.ReadAllText(cacheFilePath);
            evaluatedFrames = JsonConvert.DeserializeObject<List<EmotionFrame>>(serialisedResult);

            return true;
        }

        private void StoreEvaluatedFramesInCacheFile(List<EmotionFrame> evaluatedFrames)
        {
            var serialisedResult = JsonConvert.SerializeObject(evaluatedFrames);

            var directoryPath = Path.GetDirectoryName(cacheFilePath);

            if (!Directory.Exists(directoryPath))
                Directory.CreateDirectory(directoryPath);

            File.WriteAllText(cacheFilePath, serialisedResult);
        }
    }
}
