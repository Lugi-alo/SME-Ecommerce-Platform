import nltk
nltk.download('punkt')
nltk.download('vader_lexicon')

from nltk.sentiment import SentimentIntensityAnalyzer

def analyseText(reviewText):
    tokens = nltk.word_tokenize(reviewText)

    sia = SentimentIntensityAnalyzer()
    sentimentScore = sia.polarity_scores(reviewText)['compound']
    return tokens, sentimentScore

