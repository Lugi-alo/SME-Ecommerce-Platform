using System;
using Python.Runtime;

namespace Dissertation.Models
{
    public class TextAnalyser
    {
        public double AnalyseSentiment(string text)
        {
            using (Py.GIL())
            {
                dynamic nltk = Py.Import("nltk");
                dynamic sia = nltk.sentiment.SentimentIntensityAnalyzer();

                dynamic result = sia.polarity_scores(text);
                double compoundScore = result["compound"];

                return compoundScore;
            }
        }
    }
}
