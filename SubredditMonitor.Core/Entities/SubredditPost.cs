namespace SubredditMonitor.Core.Entities
{
    public class SubredditPost
    {
        public string? PostID { get; set; }
        public string? Title { get; set; }
        public string? AuthorUserId { get; set; }
        public string? AuthorName { get; set; }
        public int Upvotes { get; set; }
    }
}
