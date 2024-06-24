namespace SubredditMonitor.Core.Entities
{
    public class SubredditRequestInfo
    {
        public string? after { get; set; }
        public List<LinkDataList>? children { get; set; }
    }
}
