namespace Dissertation.Models
{
    public class ServiceListings
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public string Country { get; set; }
        public string Image { get; set; }
        public string ListingType { get; set; }

        public string BusinessId { get; set; }
        public List<Review> Reviews { get; set; }

        public ICollection<ServiceListingFeature> ServiceListingFeatures { get; set; }

        public double CalculateAverageRating()
        {
            if (Reviews == null || Reviews.Count == 0)
            {
                return 0;
            }

            int totalRating = Reviews.Sum(r => r.Rating);
            double averageRating = (double)totalRating / Reviews.Count;

            return averageRating;
        }

        public string CalculateAverageSentiment(List<Review> reviews)
        {

            if (reviews == null || reviews.Count == 0)
            {
                return "No reviews";
            }

            double totalSentimentScore = 0;
            foreach (var review in reviews)
            {
                totalSentimentScore += review.SentimentScore;
            }
            double averageSentimentScore = totalSentimentScore / reviews.Count;

            double threshold = 0.5;
            string overallSentiment;

            if (averageSentimentScore > threshold)
            {
                overallSentiment = "Positive";
            }
            else
            {
                overallSentiment = "Negative";
            }

            return overallSentiment;
        }
    }
}
