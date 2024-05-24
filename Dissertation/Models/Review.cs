namespace Dissertation.Models
{
    public class Review
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string ReviewText { get; set; }
        public int Rating { get; set; }
        public DateTime Timestamp { get; set; }
        public double SentimentScore { get; set; }

        public int ServiceId { get; set; }
        public ServiceListings ServiceListings { get; set; }



    }
}