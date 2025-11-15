using Azure.AI.OpenAI;
using Azure;
using Fall2025_Project3_nrmiller4.Models;
using VaderSharp2;
using OpenAI.Chat;

namespace Fall2025_Project3_nrmiller4.Services
{
    public class AIService : IAIService
    {
        private readonly AzureOpenAIClient _client;
        private readonly string _deploymentName;
        private readonly SentimentIntensityAnalyzer _sentimentAnalyzer;

        public AIService(IConfiguration configuration)
        {
            var endpoint = configuration["AzureOpenAI:Endpoint"] 
                ?? throw new InvalidOperationException("Azure OpenAI endpoint not configured");
            var apiKey = configuration["AzureOpenAI:ApiKey"] 
                ?? throw new InvalidOperationException("Azure OpenAI API key not configured");
            _deploymentName = configuration["AzureOpenAI:DeploymentName"] 
                ?? throw new InvalidOperationException("Azure OpenAI deployment name not configured");

            _client = new AzureOpenAIClient(new Uri(endpoint), new AzureKeyCredential(apiKey));
            _sentimentAnalyzer = new SentimentIntensityAnalyzer();
        }

        public async Task<List<ReviewWithSentiment>> GenerateMovieReviewsAsync(string movieTitle, int count = 10)
        {
            var chatClient = _client.GetChatClient(_deploymentName);

            var prompt = $"Generate exactly {count} short movie reviews (2-3 sentences each) for the movie '{movieTitle}'. " +
                        $"Each review should be on a new line. Include a mix of positive, negative, and neutral reviews. " +
                        $"Number each review (1-{count}).";

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are a movie critic generating diverse reviews."),
                new UserChatMessage(prompt)
            };

            var response = await chatClient.CompleteChatAsync(messages);
            var content = response.Value.Content[0].Text;

            // Parse reviews
            var reviews = new List<ReviewWithSentiment>();
            var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var cleanLine = line.Trim();
                // Remove numbering if present
                if (cleanLine.Length > 3 && char.IsDigit(cleanLine[0]))
                {
                    cleanLine = cleanLine.Substring(cleanLine.IndexOf('.') + 1).Trim();
                }

                if (!string.IsNullOrWhiteSpace(cleanLine) && reviews.Count < count)
                {
                    var sentiment = _sentimentAnalyzer.PolarityScores(cleanLine);
                    reviews.Add(new ReviewWithSentiment
                    {
                        Review = cleanLine,
                        Sentiment = sentiment.Compound,
                        SentimentLabel = GetSentimentLabel(sentiment.Compound)
                    });
                }
            }

            // Ensure we have exactly the requested count
            while (reviews.Count < count)
            {
                reviews.Add(new ReviewWithSentiment
                {
                    Review = $"A {(reviews.Count % 2 == 0 ? "great" : "decent")} movie worth watching.",
                    Sentiment = 0.5,
                    SentimentLabel = "Positive"
                });
            }

            return reviews.Take(count).ToList();
        }

        public async Task<List<TweetWithSentiment>> GenerateActorTweetsAsync(string actorName, int count = 20)
        {
            var chatClient = _client.GetChatClient(_deploymentName);

            var prompt = $"Generate exactly {count} short tweets (280 characters or less each) about the actor '{actorName}'. " +
                        $"Each tweet should be on a new line. Include a mix of positive comments, criticism, and neutral observations. " +
                        $"Make them sound like real tweets from fans and critics. Number each tweet (1-{count}).";

            var messages = new List<ChatMessage>
            {
                new SystemChatMessage("You are generating Twitter-style comments about actors."),
                new UserChatMessage(prompt)
            };

            var response = await chatClient.CompleteChatAsync(messages);
            var content = response.Value.Content[0].Text;

            // Parse tweets
            var tweets = new List<TweetWithSentiment>();
            var lines = content.Split('\n', StringSplitOptions.RemoveEmptyEntries);

            foreach (var line in lines)
            {
                var cleanLine = line.Trim();
                // Remove numbering if present
                if (cleanLine.Length > 3 && char.IsDigit(cleanLine[0]))
                {
                    cleanLine = cleanLine.Substring(cleanLine.IndexOf('.') + 1).Trim();
                }

                if (!string.IsNullOrWhiteSpace(cleanLine) && tweets.Count < count)
                {
                    var sentiment = _sentimentAnalyzer.PolarityScores(cleanLine);
                    tweets.Add(new TweetWithSentiment
                    {
                        Tweet = cleanLine,
                        Sentiment = sentiment.Compound,
                        SentimentLabel = GetSentimentLabel(sentiment.Compound)
                    });
                }
            }

            // Ensure we have exactly the requested count
            while (tweets.Count < count)
            {
                tweets.Add(new TweetWithSentiment
                {
                    Tweet = $"#{actorName} is {(tweets.Count % 2 == 0 ? "amazing" : "talented")} in their latest role!",
                    Sentiment = 0.6,
                    SentimentLabel = "Positive"
                });
            }

            return tweets.Take(count).ToList();
        }

        private string GetSentimentLabel(double compound)
        {
            if (compound >= 0.05)
                return "Positive";
            else if (compound <= -0.05)
                return "Negative";
            else
                return "Neutral";
        }
    }
}
