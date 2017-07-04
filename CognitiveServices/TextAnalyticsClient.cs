namespace CognitiveServices
{
    using Newtonsoft.Json;
    using Parsing_Classes.Key_Phrases;
    using Parsing_Classes.Sentiment;
    using System.Collections.Generic;
    using System.Configuration;
    using System.Net.Http;
    using System.Text;
    using System.Threading.Tasks;

    public class TextAnalyticsClient
    {
        public enum Sentiment { Positive, Negative, Neutral };

        private int sentenceCount;
        private string documents;

        private double negativeThreshold = 0.4;
        private double positiveThreshold = 0.6;

        private const string sentimentUri = @"https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/sentiment";
        private const string keyPhrasesUri = @"https://westus.api.cognitive.microsoft.com/text/analytics/v2.0/keyPhrases";

        private string bodyTemplate = "{{\"documents\":[{0}]}}";
        private string documentTemplate = "{{\"language\": \"en\", \"id\": \"{0}\",\"text\": \"{1}\"}}";

        private HttpClient client;

        public TextAnalyticsClient()
        {
            var subscriptionKey = ConfigurationManager.AppSettings["Text-Analytics-API-Sub-Key"];
            client = new HttpClient();
            client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", subscriptionKey);

            sentenceCount = 0;
            documents = string.Empty;
        }

        /// <summary>
        /// Sets the data to be sent to the API. Takes in the sentence uttered, formats it into a 'document' inside the body of the
        /// JSON to be sent, and converts into a byte array.
        /// </summary>
        /// <param name="sentence">The sentence uttered by the user.</param>
        public void SetData(string sentence)
        {
            if (sentenceCount != 0)
                documents += ",";

            documents += string.Format(documentTemplate, sentenceCount, sentence);
            sentenceCount++;
        }

        /// <summary>
        /// Sets the data to be sent to the API. Takes in the sentence uttered, formats it into a 'document' inside the body of the
        /// JSON to be sent, and converts into a byte array.
        /// </summary>
        /// <param name="sentence">The sentence uttered by the user.</param>
        public void SetData(List<string> sentences)
        {
            foreach (var sentence in sentences)
                this.SetData(sentence);
        }

        public void ResetData()
        {
            sentenceCount = 0;
            documents = string.Empty;
        }

        /// <summary>
        /// Asynchronously sends the set data to the API, deserialises the JSON response, and returns the determined sentiment.
        /// </summary>
        /// <returns>The sentiment of the analysed data. Positive. negative or neutral.</returns>
        public async Task<List<double>> GetSpeechSentimentScores()
        {
            var data = GetContent();
            var response = await client.PostAsync(sentimentUri, data);
            var respString = await response.Content.ReadAsStringAsync();

            var sentimentScores = new List<double>();
            var results = JsonConvert.DeserializeObject<SentimentResult>(respString).documents;

            for (var i = 0; i < results.Count; i++)
                sentimentScores.Add(results[i].score);

            return sentimentScores;
        }

        /// <summary>
        /// Asynchronously sends the set data to the API, deserialises the JSON response, and returns the determined sentiment.
        /// </summary>
        /// <returns>The sentiment of the analysed data. Positive. negative or neutral.</returns>
        public async Task<Sentiment> GetSentenceSentiment()
        {
            var data = GetContent();
            var response = await client.PostAsync(sentimentUri, data);
            var respString = await response.Content.ReadAsStringAsync();

            var sentimentScore = JsonConvert.DeserializeObject<SentimentResult>(respString).documents[0].score;

            return GetSentiment(sentimentScore);
        }

        private HttpContent GetContent()
        {
            var body = string.Format(bodyTemplate, documents);
            var dataBytes = Encoding.UTF8.GetBytes(body);
            var data = new ByteArrayContent(dataBytes);
            data.Headers.ContentType = new System.Net.Http.Headers.MediaTypeHeaderValue("application/json");

            return data;
        }

        private Sentiment GetSentiment(double sentimentScore)
        {
            if (sentimentScore < negativeThreshold)
                return Sentiment.Negative;
            if (sentimentScore > positiveThreshold)
                return Sentiment.Positive;
            return Sentiment.Neutral;
        }

        public async Task<string[]> GetSentenceKeyPhrases()
        {
            var data = GetContent();
            var response = await client.PostAsync(keyPhrasesUri, data);
            var respString = await response.Content.ReadAsStringAsync();

            return JsonConvert.DeserializeObject<KeyPhrasesResult>(respString).documents[0].keyPhrases;
        }
    }
}
