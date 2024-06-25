using SubredditMonitor.Core.Interfaces;
using Microsoft.Extensions.Configuration;

namespace SubredditMonitor.Infrastructure.Data.Config
{
    public class ApplicationSettings : IApplicationSettings
    {
        private readonly Dictionary<string, string> _appSettings = [];

        public ApplicationSettings(IConfiguration configuration)
        {
            var authsect = configuration.GetSection("Authentication");

            if (authsect == null) throw new ArgumentNullException("ERROR! Reddit API Authentication section not configured correctly.");
    
            _appSettings.Add("ClientID", authsect.GetSection("ClientID").Value ?? "") ;
            _appSettings.Add("ClientSecret", authsect.GetSection("ClientSecret").Value ?? "");

            _appSettings.Add("RedditAuthUri", "https://www.reddit.com/api/v1/access_token");
            _appSettings.Add("RedditApiBaseUri", "https://oauth.reddit.com");
            _appSettings.Add("Subreddit", "/r/nfl,/r/askreddit");
            _appSettings.Add("DelayBetweenStatusUpdatesInSeconds", "10");
        }

        public string GetSettingValue(string key)
        {
            return _appSettings[key];
        }
    }
}
