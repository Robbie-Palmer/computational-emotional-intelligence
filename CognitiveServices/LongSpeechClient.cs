/// LongSpeechClient.cs is an implementation of SpeechClient which is
/// designed to take an audio file and return the words uttered in a long
/// speech as text.
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
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public class LongSpeechClient : SpeechClient
    {
        private List<string> parsedSpeech;

        public LongSpeechClient() : base()
        {
            dataClient.OnResponseReceived += this.OnDataDictationResponseReceivedHandler;

            parsedSpeech = new List<string>();
        }

        public async Task<List<string>> ParseAudioToSpeech()
        {
            await audioProcessed.WaitAsync();
            return parsedSpeech;
        }

        /// <summary>
        /// Called when a final response is received;
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SpeechResponseEventArgs"/> instance containing the event data.</param>
        private void OnDataDictationResponseReceivedHandler(object sender, SpeechResponseEventArgs e)
        {
            foreach (var response in e.PhraseResponse.Results)
            {
                parsedSpeech.Add(response.DisplayText);
            }

            if (e.PhraseResponse.RecognitionStatus == RecognitionStatus.EndOfDictation ||
                e.PhraseResponse.RecognitionStatus == RecognitionStatus.DictationEndSilenceTimeout)
            {
                audioProcessed.Release();
            }
        }
    }
}
