namespace Fall2025_Project3_nrmiller4.Models
{
    public class ActorDetailsViewModel
    {
        public Actor Actor { get; set; } = null!;
        public List<Movie> Movies { get; set; } = new();
        public List<TweetWithSentiment> Tweets { get; set; } = new();
        public double AverageSentiment { get; set; }
    }

    public class TweetWithSentiment
    {
        public string Tweet { get; set; } = string.Empty;
        public double Sentiment { get; set; }
        public string SentimentLabel { get; set; } = string.Empty;
    }
}
