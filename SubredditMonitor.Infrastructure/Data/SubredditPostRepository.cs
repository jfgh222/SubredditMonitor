using SubredditMonitor.Core.Entities;
using SubredditMonitor.Core.Interfaces;
using SubredditMonitor.Core.Services;

namespace SubredditMonitor.Infrastructure;

public class SubredditPostRepository : ISubredditPostRepository 
{
    private List<SubredditPost> _allPosts = [];
    
    public List<SubredditPost> GetAllPostsBySubreddit(string subreddit)
    {
        return _allPosts.Where(ap => ap.Subreddit == subreddit).ToList(); 
    }

    public void UpsertResponsePosts(string Subreddit, List<LinkDataList> newSubredditPosts)
    {
        foreach (var newPost in newSubredditPosts)
        {
            var linkData = newPost.data;

            if (linkData != null)
            {
                var currPost = _allPosts.FirstOrDefault(ap => ap.PostID == linkData.id);

                if (currPost == null)
                {
                    _allPosts.Add(MapRedditLinkDataToPostInfo.Map(Subreddit, linkData));
                }
                else if (currPost.Upvotes != linkData.ups)
                {
                    currPost.Upvotes = linkData.ups;
                }
            }
        }
    }
}
