/// SpeechClient.cs is an abstract class, which provides the
/// basis for implementations which use Microsoft's Bing Speech API
/// for speech to text purposes.
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

namespace CognitiveServices
{
    using Microsoft.CognitiveServices.SpeechRecognition;
    using System;
    using System.Configuration;
    using System.IO;
    using System.Threading;

    public abstract class SpeechClient
    {
        protected SemaphoreSlim audioProcessed = new SemaphoreSlim(0, 1);
        protected DataRecognitionClient dataClient;

        private const string defaultLocation = "en-us";

        public SpeechClient()
        {
            var subscriptionKey = ConfigurationManager.AppSettings["Speech-API-Sub-Key"];
            dataClient = SpeechRecognitionServiceFactory.CreateDataClient(SpeechRecognitionMode.LongDictation, defaultLocation, subscriptionKey);
            dataClient.OnConversationError += this.OnConversationErrorHandler;
        }

        public SpeechClient(string luisAppId, string luisSubId)
        {
            var subscriptionKey = ConfigurationManager.AppSettings["Speech-API-Sub-Key"];
            dataClient = SpeechRecognitionServiceFactory.CreateDataClientWithIntent(
                defaultLocation,
                subscriptionKey,
                luisAppId,
                luisSubId);
            dataClient.OnConversationError += this.OnConversationErrorHandler;
        }

        /// <summary>
        /// Sends the audio to the API.
        /// </summary>
        /// <param name="inputPath">The filepath of the input video.</param>
        public void SendAudio(string inputPath)
        {
            if(!File.Exists(inputPath))
            {
                throw new FileNotFoundException();
            }

            var wavFilePath = inputPath.Replace("mp4", "wav");

            if (!File.Exists(wavFilePath))
            {
                var fileNameLength = Path.GetFileName(inputPath).Length;
                var directoryPath = inputPath.Remove(inputPath.Length - fileNameLength);

                WavFileGenerator.GenerateMissingWav(directoryPath, Path.GetFileNameWithoutExtension(inputPath));
            }
            
            using (FileStream fileStream = new FileStream(wavFilePath, FileMode.Open, FileAccess.Read))
            {
                int bytesRead = 0;
                byte[] buffer = new byte[1024];

                try
                {
                    do
                    {
                        // Get more Audio data to send into byte buffer.
                        bytesRead = fileStream.Read(buffer, 0, buffer.Length);

                        // Send of audio data to service. 
                        this.dataClient.SendAudio(buffer, bytesRead);
                    }
                    while (bytesRead > 0);
                }
                finally
                {
                    // We are done sending audio.  Final recognition results will arrive in OnResponseReceived event call.
                    this.dataClient.EndAudio();
                }
            }
        }

        public void Dispose()
        {
            this.dataClient.Dispose();
        }

        /// <summary>
        /// Called when an error is received.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SpeechErrorEventArgs"/> instance containing the event data.</param>
        private void OnConversationErrorHandler(object sender, SpeechErrorEventArgs e)
        {
            Console.WriteLine("--- Error received by OnConversationErrorHandler() ---");
            Console.WriteLine("Error code: {0}", e.SpeechErrorCode.ToString());
            Console.WriteLine("Error text: {0}", e.SpeechErrorText);
            Console.WriteLine();

            // Exit application
            Environment.Exit(0);
        }
    }
}
