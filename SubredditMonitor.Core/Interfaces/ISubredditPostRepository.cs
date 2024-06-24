using SubredditMonitor.Core.Entities;

namespace SubredditMonitor.Core.Interfaces
{
    public interface ISubredditPostRepository
    {
        public void UpsertResponsePosts(string Subreddit, List<LinkDataList> newSubredditPosts);
        public List<SubredditPost> GetAllPostsBySubreddit(string subreddit);
    }

}
