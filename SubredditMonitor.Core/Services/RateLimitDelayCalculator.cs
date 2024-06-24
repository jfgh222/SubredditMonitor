namespace SubredditMonitor.Core.Services
{
    public static class RateLimitDelayCalculator
    {
        public static int CalculateRequestDelayBasedOnRateLimits(IReadOnlyCollection<RestSharp.HeaderParameter> headers)
        {
            if (headers == null) throw new ArgumentNullException(nameof(headers));
            if (headers.Count == 0) throw new ArgumentException(nameof(headers));

            var rateLimitRemaining = headers.FirstOrDefault(h => h.Name == "x-ratelimit-remaining")?.Value?.ToString();
            var rateLimitReset = headers.FirstOrDefault(h => h.Name == "x-ratelimit-reset")?.Value?.ToString();

            if (rateLimitRemaining != null && rateLimitReset != null)
            {
                return CalculateDelay(rateLimitRemaining, rateLimitReset);
            }

            return 0;
        }

        public static int CalculateDelay(string remainingRequests, string resetSeconds)
        {
            double iRemaining = 0;
            double iReset = 0;

            if (double.TryParse(remainingRequests, out iRemaining))
            {
                if (double.TryParse(resetSeconds, out iReset))
                {
                    var requestsPerSecondUntilReset = iRemaining / iReset;
                    var secondsUntilNextRequestAllowed = requestsPerSecondUntilReset > 0 ? 1 / requestsPerSecondUntilReset : iReset;
                    return Math.Max((int)(secondsUntilNextRequestAllowed * 1000), 0);
                }
            }

            return 0;
        }
    }
}
