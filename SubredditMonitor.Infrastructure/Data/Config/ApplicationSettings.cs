namespace SubredditMonitor.Infrastructure.Data.Config
{
    public class ApplicationSettings
    {
        private readonly Dictionary<string, string> _appSettings = [];

        public ApplicationSettings()
        {
            GetSettingValueFromConfig("ClientID");
            GetSettingValueFromConfig("ClientSecret");

            _appSettings.Add("RedditAuthUri", "https://www.reddit.com/api/v1/access_token");
            _appSettings.Add("RedditApiBaseUri", "https://oauth.reddit.com");
            _appSettings.Add("Subreddit", "/r/nfl,/r/askreddit");
            _appSettings.Add("DelayBetweenStatusUpdatesInSeconds", "10");
        }

        public void GetSettingValueFromConfig(string key)
        {
            var settingValue = System.Configuration.ConfigurationManager.AppSettings[key];
            if (string.IsNullOrWhiteSpace(settingValue)) throw new ArgumentNullException(key);

            _appSettings.Add(key, settingValue);
        }

        public string GetSettingValue(string key)
        {
            return _appSettings[key];
        }
    }
}
