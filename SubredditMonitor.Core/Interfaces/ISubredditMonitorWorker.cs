namespace SubredditMonitor.Core.Interfaces
{
    public interface ISubredditMonitorWorker
    {
        public void MonitorSubredditPosts();
        public void SetSubreddit(string subreddit);
    }
}
