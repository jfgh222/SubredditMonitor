using RestSharp;
using SubredditMonitor.Core.Entities;
using System.Text.Json;

namespace SubredditMonitor.Core.Services
{
    public class RedditApiRequest
    {
        private string requestUri;

        private RestClient restClient;

        private static int NextRequestRateLimitDelay = 0;

        private static RestClientOptions restClientOptions = new RestClientOptions();

        public static void SetRestClientOptions(string redditApiBaseUri)
        {
            restClientOptions = new RestClientOptions(redditApiBaseUri)
            {
                Timeout = TimeSpan.FromSeconds(10)
            };
        }

        public RedditApiRequest(string requestUri) 
        {
            this.requestUri = requestUri;
            restClient = new RestClient(restClientOptions);
        }

        public async Task<Listing> GetApiResponse()
        {
            var request = new RestRequest(requestUri, Method.Get);
            request.AddHeader("Authorization", "Bearer " + RedditOAuthToken.TokenValue);

            try
            {
                Thread.Sleep(NextRequestRateLimitDelay);

                var restResponse = await restClient.ExecuteAsync(request);

                if (restResponse != null)
                {
                    if (restResponse.Headers != null)
                    {
                        NextRequestRateLimitDelay = RateLimitDelayCalculator.CalculateRequestDelayBasedOnRateLimits(restResponse.Headers);
                    }

                    var content = restResponse?.Content;
                    if (content != null)
                    {
                        var result = JsonSerializer.Deserialize<Listing>(content);
                        if (result != null) return result;
                    }
                }
            }
            catch (Exception ex) 
            {
                Console.WriteLine(ex.Message);
                throw;
            }

            return new Listing();
        }
    }
}
