﻿using SubredditMonitor.Core.Interfaces;

namespace SubredditMonitor.Core.Services
{
    public class SubredditMonitorWorker : ISubredditMonitorWorker
    {
        public string? Subreddit { get; set; }

        private ISubredditPostRepository _subredditRepo;
        private ISubredditPostRetriever _subredditPostRetriever;
        private IStatusUpdater _statusUpdater;

        public SubredditMonitorWorker(ISubredditPostRepository subredditPostRepository, ISubredditPostRetriever subredditPostRetriever, IStatusUpdater statusUpdater)
        {
            _subredditRepo = subredditPostRepository;
            _subredditPostRetriever = subredditPostRetriever;
            _statusUpdater = statusUpdater;
        }

        public async Task SetSubreddit(string subreddit)
        {
            Subreddit = subreddit;
            await _subredditPostRetriever.SetSubreddit(subreddit);
            _statusUpdater.SetSubreddit(subreddit);
        }

        public async void MonitorSubredditPosts()
        {
            if (Subreddit != null)
            {
                Console.WriteLine("Initialize monitoring of subreddit: [" + Subreddit + "]");

                new Task(_statusUpdater.ShowStatusUpdates).Start();

                while (true)
                {
                    var postsSinceStart = await _subredditPostRetriever.RetrieveAllPostsSinceAppStart();
                    _subredditRepo.UpsertResponsePosts(Subreddit, postsSinceStart);
                }
            }
        }
    }
}
