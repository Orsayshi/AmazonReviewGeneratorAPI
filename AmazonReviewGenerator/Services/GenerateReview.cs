using AmazonReviewGenerator.Models;
using ReviewGenerator.Models;
using VaderSharp2;

namespace ReviewGenerator.Services
{
    public class GenerateReview
    {
        private static Random _random = new Random();

        /// <summary>
        /// Generates new Amazon Review
        /// </summary>
        /// <param name="wordPossibilities"></param>
        /// <param name="reviewSize"></param>
        /// <returns>Amazon Review Model with Text as the review and review score as the rating</returns>
        public static AmazonReviewModel GenerateNewReview(Dictionary<string, TextPattern> wordPossibilities, int reviewSize)
        {
            List<string> review = new List<string>();
            for(int i = 0; i < reviewSize; i++)
            {
                int wordIndex = _random.Next(wordPossibilities.Count);
                var firstWord = wordPossibilities.ElementAt(wordIndex).Key;
                review.Add(firstWord);

                int nextPossibleIndex = _random.Next(wordPossibilities[firstWord].Next.Count);
                var nextWord = wordPossibilities[firstWord].Next[nextPossibleIndex];
                review.Add(nextWord);
            }
            var generatedReview = string.Join(" ", review);
            return new AmazonReviewModel()
            {
                Text = generatedReview,
                reviewScore = AnalyzeSentiment(generatedReview)
            };   
        }

        /// <summary>
        /// Using VaderSharp2 Sentinment Analyzer
        /// </summary>
        /// <param name="text"></param>
        /// <returns>Random rating between 1 and 5 based on sentiment analysis of text</returns>
        private static double AnalyzeSentiment(string text)
        {
            var analyzer = new SentimentIntensityAnalyzer();
            var results = analyzer.PolarityScores(text);
            if(results.Compound >= 0.05)
            {
                return Math.Round(_random.NextDouble() * (5 - 3) + 3, 2);
            }
            else if(results.Compound > -0.05 && results.Compound < 0.05)
            {
                return 2.5;
            }
            else
            {
                return Math.Round(_random.NextDouble() * (3 - 1) + 1, 2);
            }
        }

    }
}
