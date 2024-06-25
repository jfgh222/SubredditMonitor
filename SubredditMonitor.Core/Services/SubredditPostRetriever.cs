using SubredditMonitor.Core.Entities;

namespace SubredditMonitor.Core.Services
{
    public interface ISubredditPostRetriever
    {
        public Task SetSubreddit(string subreddit);
        public Task<string> GetMostRecentPostID();
        public Task<List<LinkDataList>> RetrieveAllPostsSinceAppStart();
    }

    public class SubredditPostRetriever : ISubredditPostRetriever
    {
        private string? _subreddit;
        private string? _firstPostID;

        private string InitialPostBeforeValue => "t3_" + _firstPostID;
        private string AllPostsBeforeInitialRequestUri => _subreddit + "/new?before=" + InitialPostBeforeValue + "&limit=1000";

        public async Task SetSubreddit(string subbreddit)
        {
            _subreddit = subbreddit;
            _firstPostID = await GetMostRecentPostID();
            if (string.IsNullOrWhiteSpace(_firstPostID)) throw new Exception("ERROR! No posts found on subreddit [" + _subreddit + "]");
        }

        public async Task<List<LinkDataList>> RetrieveAllPostsSinceAppStart()
        {
            return await GetPosts(AllPostsBeforeInitialRequestUri);
        }

        private async Task<List<LinkDataList>> GetPosts(string requestUri)
        {
            await RedditOAuthToken.CheckOAuthToken();

            var apiPostDataRequest = new RedditApiRequest(requestUri);
            var response = await apiPostDataRequest.GetApiResponse();

            if (response != null)
            {
                var postCount = 0;
                if (response?.data?.children != null)
                {
                    postCount = response.data.children.Count;
                    if (postCount > 0)
                    {
                        return response.data.children;
                    }
                }
            }

            return new List<LinkDataList>();
        }

        public async Task<string> GetMostRecentPostID()
        {
            var mostRecentUri = _subreddit + "/new?limit=1";
            await RedditOAuthToken.CheckOAuthToken();
            var mostRecentPostRequest = new RedditApiRequest(mostRecentUri);

            var response = await mostRecentPostRequest.GetApiResponse();

            return GetFirstPostID(response);
        }

        private string GetFirstPostID(Listing apiResp)
        {
            if (apiResp != null)
            {
                var id = apiResp?.data?.children?.FirstOrDefault()?.data?.id;

                return id ?? "";
            }

            return "";
        }

    }
}
