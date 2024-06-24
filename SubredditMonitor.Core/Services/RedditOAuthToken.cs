using System.Net.Http.Headers;
using System.Security.Authentication;
using System.Text;
using System.Text.Json;

namespace SubredditMonitor.Core.Services
{
    public static class RedditOAuthToken
    {
        public static string TokenValue = "";

        private static string? Base64AuthString { get; set; }
        private static HttpRequestMessage? RequestMessage { get; set; }

        private static DateTime Expires{ get; set; }

        public static bool Expired => DateTime.Now >= Expires;

        public static void ConfigureRedditOAuth(string ClientId, string ClientSecret, string RedditAuthUri)
        {
            var authString = $"{ClientId}:{ClientSecret}";
            Base64AuthString = Convert.ToBase64String(ASCIIEncoding.UTF8.GetBytes(authString));

            RequestMessage = new HttpRequestMessage(HttpMethod.Post, RedditAuthUri)
            {
                Content = new StringContent("grant_type=client_credentials", Encoding.UTF8, "application/x-www-form-urlencoded")
            };

            RequestMessage.Headers.Authorization = new AuthenticationHeaderValue("basic", Base64AuthString);
            RequestMessage.Headers.Add("user-agent", "jkf-mon-app");
        }

        public static async Task RetrieveOAuthToken()
        {
            var client = new HttpClient();

            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            if (RequestMessage != null)
            {
                var response = await client.SendAsync(RequestMessage);
                var result = await response.Content.ReadAsStringAsync();

                var deserial = JsonSerializer.Deserialize<redditApiTokenResponse>(result);

                if (deserial == null || deserial.access_token == null) throw new AuthenticationException("ERROR! Couldn't obtain OAuth token from Reddit. Please check credentials and try again ... ");

                Expires = DateTime.Now.AddSeconds(deserial.expires_in);
                TokenValue = deserial.access_token;
            }
        }

        public static async Task CheckOAuthToken()
        {
            if (Expired) { await RetrieveOAuthToken(); }
        }

        private class redditApiTokenResponse
        {
            public string? access_token { get; set; }
            public long expires_in { get; set; }
        }
    }
}
