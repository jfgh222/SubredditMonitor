using SubredditMonitor.Core.Entities;

namespace SubredditMonitor.Core.Interfaces
{
    public interface ISubredditPostRepository
    {
        public void UpsertResponsePosts(List<LinkDataList> newSubredditPosts);
        public List<SubredditPost> GetAllPosts();
    }

}
