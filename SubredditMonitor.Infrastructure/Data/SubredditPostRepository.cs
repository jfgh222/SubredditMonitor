using SubredditMonitor.Core.Entities;
using SubredditMonitor.Core.Interfaces;
using SubredditMonitor.Core.Services;

namespace SubredditMonitor.Infrastructure;

public class SubredditPostRepository : ISubredditPostRepository 
{
    private List<SubredditPost> _allPosts = [];
    
    public List<SubredditPost> GetAllPosts()
    {
        return _allPosts; 
    }

    public void UpsertResponsePosts(List<LinkDataList> newSubredditPosts)
    {
        foreach (var newPost in newSubredditPosts)
        {
            var linkData = newPost.data;

            if (linkData != null)
            {
                var currPost = _allPosts.FirstOrDefault(ap => ap.PostID == linkData.id);

                if (currPost == null)
                {
                    _allPosts.Add(MapRedditLinkDataToPostInfo.Map(linkData));
                }
                else if (currPost.Upvotes != linkData.ups)
                {
                    currPost.Upvotes = linkData.ups;
                }
            }
        }
    }
}
