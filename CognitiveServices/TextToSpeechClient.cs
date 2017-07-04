/// TextToSpeechClient.cs provides an implementation for using
/// Microsoft's Bing Speech API to convert text to speech.
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
    using System;
    using CognitiveServicesTTS;
    using System.Threading;
    using System.IO;
    using System.Threading.Tasks;
    using System.Media;
    using System.Configuration;

    public class TextToSpeechClient
    {
        private SemaphoreSlim textProcessed = new SemaphoreSlim(0, 1);
        
        private const string requestUri = "https://speech.platform.bing.com/synthesize";

        private Stream audioResponse;
        private Synthesize speaker;
        private Synthesize.InputOptions speakerSettings;

        public TextToSpeechClient()
        {
            speaker = new Synthesize();

            speaker.OnAudioAvailable += OnAudioReceivedHandler;
            speaker.OnError += ErrorHandler;

            var accessToken = GetAccessToken();

            speakerSettings = new Synthesize.InputOptions()
            {
                RequestUri = new Uri(requestUri),
                VoiceType = Gender.Female,
                Locale = "en-US",
                VoiceName = "Microsoft Server Speech Text to Speech Voice (en-US, ZiraRUS)",
                OutputFormat = AudioOutputFormat.Riff16Khz16BitMonoPcm,
                AuthorizationToken = "Bearer " + accessToken,
            };
        }

        public void SetTextToSay(string text)
        {
            speakerSettings.Text = text;
        }

        public async Task ProcessTextToSpeech()
        {
            await speaker.Speak(CancellationToken.None, speakerSettings);
            await textProcessed.WaitAsync();
        }

        public void Speak()
        {
            SoundPlayer player = new SoundPlayer(audioResponse);
            player.PlaySync();
        }

        public void Dispose()
        {
            if(audioResponse != null)
                audioResponse.Dispose();
        }

        private void OnAudioReceivedHandler(object sender, GenericEventArgs<Stream> response)
        {
            audioResponse = response.EventData;
            textProcessed.Release();
        }

        /// <summary>
        /// Handler an error when a TTS request failed.
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="GenericEventArgs{Exception}"/> instance containing the event data.</param>
        private static void ErrorHandler(object sender, GenericEventArgs<Exception> e)
        {
            Console.WriteLine("Unable to complete the TTS request: [{0}]", e.ToString());
        }

        private static string GetAccessToken()
        {
            var subscriptionKey = ConfigurationManager.AppSettings["Speech-API-Sub-Key"];
            var auth = new Authentication(subscriptionKey);

            string accessToken = null;

            try
            {
                accessToken = auth.GetAccessToken();
            }
            catch (Exception ex)
            {
                Console.WriteLine("Failed authentication.");
                Console.WriteLine(ex.ToString());
                Console.WriteLine(ex.Message);
            }

            return accessToken;
        }
    }
}
