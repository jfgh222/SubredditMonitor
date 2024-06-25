namespace SubredditMonitor.Core.Interfaces
{
    public interface IApplicationSettings
    {
        public string GetSettingValue(string key);
    }
}
