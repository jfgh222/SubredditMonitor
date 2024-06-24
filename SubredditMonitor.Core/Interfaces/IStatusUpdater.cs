namespace SubredditMonitor.Core.Interfaces
{
    public interface IStatusUpdater
    {
        public void ShowStatusUpdates();
        public void SetSubreddit(string subreddit);
    }
}
