using SubredditMonitor.Core.Entities;

namespace SubredditMonitor.Core.Services
{
    public static class MapRedditLinkDataToPostInfo
    {
        public static SubredditPost Map(string Subreddit, LinkData linkData)
        {
            return new SubredditPost
            {
                Subreddit = Subreddit,
                PostID = linkData.id,
                AuthorUserId = linkData.author_fullname,
                AuthorName = linkData.author,
                Title = linkData.title,
                Upvotes = linkData.ups
            };
        }
    }
}
