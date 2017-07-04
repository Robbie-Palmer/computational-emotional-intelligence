/// SpeechIntentClient.cs is an implementation of SpeechClient.cs which
/// takes text, and by integrating with LUIS determines an intent from
/// a short statement.
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
    using Parsing_Classes.LUIS_Intent;
    using Newtonsoft.Json;
    using System.Threading.Tasks;
    using System;
    using Microsoft.CognitiveServices.SpeechRecognition;

    public class SpeechIntentClient : SpeechClient
    {
        const string luisAppId = "37d43cda-7fab-4a8a-9ea4-b7726b853868";
        const string luisSubId = "e5b898635c3a4ceb98fb584c122f98de";

        private string intentResponse;

        public SpeechIntentClient() : base(luisAppId, luisSubId)
        {
            // Event handlers for speech recognition results and intent result
            dataClient.OnResponseReceived += this.OnDataShortPhraseResponseReceivedHandler;
            dataClient.OnIntent += this.OnIntentHandler;

            intentResponse = string.Empty;
        }

        public async Task<LUISResponse> DetermineSpeechAndIntent()
        {
            await audioProcessed.WaitAsync();
            return JsonConvert.DeserializeObject<LUISResponse>(intentResponse);
        }
        
        /// <summary>
        /// Called when a final response is received;
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SpeechResponseEventArgs"/> instance containing the event data.</param>
        private void OnDataShortPhraseResponseReceivedHandler(object sender, SpeechResponseEventArgs e)
        {
            if (e.PhraseResponse.Results.Length == 0)
            {
                Console.WriteLine("No phrase response is available.");
                return;
            }
        }

        /// <summary>
        /// Called when a final response is received and its intent is parsed
        /// </summary>
        /// <param name="sender">The sender.</param>
        /// <param name="e">The <see cref="SpeechIntentEventArgs"/> instance containing the event data.</param>
        private void OnIntentHandler(object sender, SpeechIntentEventArgs e)
        {
            // Look at e.Intent???
            intentResponse = e.Payload;
            audioProcessed.Release();
        }
    }
}
