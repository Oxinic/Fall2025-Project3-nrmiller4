namespace Fall2025_Project3_nrmiller4.Models
{
    public class MovieDetailsViewModel
    {
        public Movie Movie { get; set; } = null!;
        public List<Actor> Actors { get; set; } = new();
        public List<ReviewWithSentiment> Reviews { get; set; } = new();
        public double AverageSentiment { get; set; }
    }

    public class ReviewWithSentiment
    {
        public string Review { get; set; } = string.Empty;
        public double Sentiment { get; set; }
        public string SentimentLabel { get; set; } = string.Empty;
    }
}
