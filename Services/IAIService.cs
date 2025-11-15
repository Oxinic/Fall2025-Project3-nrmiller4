using Fall2025_Project3_nrmiller4.Models;

namespace Fall2025_Project3_nrmiller4.Services
{
    public interface IAIService
    {
        Task<List<ReviewWithSentiment>> GenerateMovieReviewsAsync(string movieTitle, int count = 10);
        Task<List<TweetWithSentiment>> GenerateActorTweetsAsync(string actorName, int count = 20);
    }
}
