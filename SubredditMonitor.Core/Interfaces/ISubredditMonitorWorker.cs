namespace SubredditMonitor.Core.Interfaces
{
    public interface ISubredditMonitorWorker
    {
        public void MonitorSubredditPosts();
        public Task SetSubreddit(string subreddit);
    }
}
