/// ResponseGenerator.cs determines the string with which to respond to
/// the user.
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

namespace ChatBot
{
    using static CognitiveServices.EmotionDetectionClient;
    using static CognitiveServices.TextAnalyticsClient;

    class ResponseGenerator
    {
        const string INTENT_WHY = "Question Why";
        const string INTENT_WHO = "Question Who";
        const string INTENT_WHERE = "Question Where";
        const string INTENT_WHAT = "Question What";
        const string INTENT_HOW = "Question How";
        const string INTENT_DESCRIBE_AGENT = "Describe Agent";

        public static string GetResponse(string userIntent, Emotion emotion, Sentiment sentiment)
        {
            switch (userIntent)
            {
                case INTENT_WHAT:
                    return GetResponseWhat(emotion);
                case INTENT_WHY:
                    return GetResponseWhy(emotion);
                case INTENT_WHO:
                    return GetResponseWho(emotion);
                case INTENT_WHERE:
                    return GetResponseWhere(emotion);
                case INTENT_HOW:
                    return GetResponseHow(emotion);
                case INTENT_DESCRIBE_AGENT:
                    return GetResponseDescribeAgent(emotion, sentiment);
                default:
                    return "Intent is not handled";
            }
        }

        // "You're brilliant", "You're awful", "You're OK", "You're not great"
        private static string GetResponseDescribeAgent(Emotion emotion, Sentiment sentiment)
        {
            switch (emotion)
            {
                case Emotion.Happiness:

                    // If words are positive and expressing happiness, assume very positive comment
                    if (sentiment == Sentiment.Positive)
                        return "Thank-you very much. I like you too.";

                    // If words are negative but expressing happiness, assume comedic teasing
                    if (sentiment == Sentiment.Negative)
                        return "I know you love me really.";

                    // If words are neutral and expressing happiness, assume positive comment
                    return "I'm here to do my best.";

                case Emotion.Sadness:

                    // If words are positive but expressing sadness, assume sarcastic/snarky comment
                    if (sentiment == Sentiment.Positive)
                        return "I try my best you know...";

                    // If words are negative and expressing sadness, assume very negative comment
                    if (sentiment == Sentiment.Negative)
                        return "I will do all I can to learn from this and not do it again.";

                    // If words are neutral and expressing sadness, assume negative comment
                    return "I hope I can be better.";

                case Emotion.Surprise:

                    // If words are positive and expressing surprise, assume dramatic positive
                    if (sentiment == Sentiment.Positive)
                        return "I'm very pleased I could help!";

                    // If words are negative and expressing surprise, assume dramatic negative
                    if (sentiment == Sentiment.Negative)
                        return "I'm sorry I displeased you.";

                    // If words are neutral and expressing surprise, assume neutral comment
                    return "What did I do that was so shocking?";

                default:

                    // If words are positive and expressing neutrality, assume positive comment
                    if (sentiment == Sentiment.Positive)
                        return "I'm here to do my best.";

                    // If words are negative and expressing neutrality, assume negative comment
                    if (sentiment == Sentiment.Negative)
                        return "I hope I can be better.";

                    // If words are neutral and expressing neutrality, assume neutral comment
                    return "Well, I like you, and I hope some day you like me too.";
            }
        }

        // "What are you doing?"
        private static string GetResponseWhat(Emotion emotion)
        {
            switch (emotion)
            {
                case Emotion.Happiness:
                    return "Am I doing something silly?";
                case Emotion.Sadness:
                    return "I'm sorry, I'll stop it now.";
                case Emotion.Surprise:
                    return "Am I doing something shocking?";
                default:
                    return "I am following your commands to the best of my ability.";
            }
        }

        // "Why would you do that?"
        private static string GetResponseWhy(Emotion emotion)
        {
            switch (emotion)
            {
                case Emotion.Happiness:
                    return "Cause I like to have fun!";
                case Emotion.Sadness:
                    return "I didn't mean for it to upset you.";
                case Emotion.Surprise:
                    return "Whoops. I thought that was what you meant.";
                default:
                    return "I believed that is what you asked of me.";
            }
        }

        // "Who said that?"
        private static string GetResponseWho(Emotion emotion)
        {
            switch (emotion)
            {
                case Emotion.Happiness:
                    return "A very funny guy said that.";
                case Emotion.Sadness:
                    return "I don't believe they meant to say something spiteful.";
                case Emotion.Surprise:
                    return "I believe it is true, am I wrong?";
                default:
                    return "I may have said it.";
            }
        }

        // "Where did I put it?"
        private static string GetResponseWhere(Emotion emotion)
        {
            switch (emotion)
            {
                case Emotion.Happiness:
                    return "You keep misplacing things, you silly man.";
                case Emotion.Sadness:
                    return "Don't worry, I'm sure it will turn up soon.";
                case Emotion.Surprise:
                    return "You don't have any idea at all?";
                default:
                    return "Wherever you left it.";
            }
        }

        // "How did you do that?"
        private static string GetResponseHow(Emotion emotion)
        {
            switch (emotion)
            {
                case Emotion.Happiness:
                    return "I can do lots of other fun things too.";
                case Emotion.Sadness:
                    return "Should I not be able to do that? I'm sorry...";
                case Emotion.Surprise:
                    return "I have many surprises in store for you.";
                default:
                    return "I do what I do, using many years training and clever algorithms.";
            }
        }
    }
}
