using SubredditMonitor.Core.Interfaces;
using SubredditMonitor.Core.Services;

namespace SubredditMonitor.Service
{
    public class SubredditMonitorService : BackgroundService
    {
        private static List<ISubredditMonitorWorker> AllSubreddits = [];

        private IServiceProvider _serviceProvider;
        private IApplicationSettings _applicationSettings;

        public SubredditMonitorService(IServiceProvider serviceProvider, IApplicationSettings applicationSettings)
        {
            _serviceProvider = serviceProvider;
            _applicationSettings = applicationSettings;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                RedditOAuthToken.ConfigureRedditOAuth(_applicationSettings.GetSettingValue("ClientID"), _applicationSettings.GetSettingValue("ClientSecret"), _applicationSettings.GetSettingValue("RedditAuthUri"));
                RedditApiRequest.SetRestClientOptions(_applicationSettings.GetSettingValue("RedditApiBaseUri"));

                var allSubReddits = _applicationSettings.GetSettingValue("Subreddit").Split(",");

                foreach (var subreddit in allSubReddits)
                {
                    var worker = _serviceProvider.GetService<ISubredditMonitorWorker>();
                    if (worker != null)
                    {
                        await worker.SetSubreddit(subreddit);
                        AllSubreddits.Add(worker);
                    }
                }

                Parallel.ForEach(AllSubreddits, sr =>
                {
                    new Task(sr.MonitorSubredditPosts).Start();
                });

                Console.WriteLine("Subreddit Monitor started up successfully. Currently monitoring " + AllSubreddits.Count + " subreddits." + Environment.NewLine);
            }
            catch (Exception ex)
            {
                Console.WriteLine("ERROR! Couldn't start SubredditMonitorService. Error was: [" + ex.Message.ToString() + "]");
            }
        }
    }
}
