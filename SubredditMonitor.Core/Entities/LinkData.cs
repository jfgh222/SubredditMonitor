namespace SubredditMonitor.Core.Entities
{
    public class LinkData
    {
        public string? id { get; set; }
        public string? title{ get; set; }
        public string? author { get; set; }
        public string? author_fullname { get; set; }
        public int ups { get; set; }
    }
}
