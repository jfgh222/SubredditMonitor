using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SubredditMonitor.Core.Interfaces;
using SubredditMonitor.Core.Services;
using SubredditMonitor.Infrastructure.Data.Config;

namespace SubredditMonitor.Service
{
    public class SubredditMonitorService : BackgroundService
    {
        private static List<ISubredditMonitorWorker> AllSubreddits = [];

        private IServiceProvider _serviceProvider;

        public SubredditMonitorService(IServiceProvider serviceProvider)
        {
            _serviceProvider = serviceProvider;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            try
            {
                var appSettings = new ApplicationSettings();

                RedditOAuthToken.ConfigureRedditOAuth(appSettings.GetSettingValue("ClientID"), appSettings.GetSettingValue("ClientSecret"), appSettings.GetSettingValue("RedditAuthUri"));
                RedditApiRequest.SetRestClientOptions(appSettings.GetSettingValue("RedditApiBaseUri"));

                var allSubReddits = appSettings.GetSettingValue("Subreddit").Split(",");

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
                Console.WriteLine("ERROR! Couldn't start SubredditMonitorService. Error was: [" + ex.ToString() + "]");
            }
        }
    }
}
