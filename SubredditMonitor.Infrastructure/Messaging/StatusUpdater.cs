﻿using SubredditMonitor.Core.Entities;
using SubredditMonitor.Core.Interfaces;

namespace SubredditMonitor.Infrastructure.Messaging
{
    public class StatusUpdater : IStatusUpdater
    {
        private string? _subreddit;
        private ISubredditPostRepository _subredditPostRepo;
        private int DelayBetweenStatusUpdatesInSeconds { get; set; }

        public StatusUpdater(IApplicationSettings appSettings, ISubredditPostRepository subredditPostRepository)
        {
            _subredditPostRepo = subredditPostRepository;

            DelayBetweenStatusUpdatesInSeconds = int.TryParse(appSettings.GetSettingValue("DelayBetweenStatusUpdatesInSeconds"), out int delay)
            ? DelayBetweenStatusUpdatesInSeconds = delay
            : 10;
        }

        public void SetSubreddit(string subreddit)
        {
            _subreddit = subreddit;
        }

        public void ShowStatusUpdates()
        {
            var mostPostsUpdateMessage = "";
            var mostUpvotesUpdateMessage = "";

            Console.WriteLine(Environment.NewLine);

            while (true && !string.IsNullOrWhiteSpace(_subreddit))
            {
                bool printedUpdate = false;

                var allPosts = _subredditPostRepo.GetAllPostsBySubreddit(_subreddit);

                var latestMostPosts = GetMostProlificString(allPosts);

                if (!string.IsNullOrWhiteSpace(latestMostPosts) && latestMostPosts != mostPostsUpdateMessage)
                {
                    mostPostsUpdateMessage = latestMostPosts;
                    Console.WriteLine(mostPostsUpdateMessage);
                    printedUpdate = true;
                }

                var latestMostUpvotes = GetMostUpvotesString(allPosts);

                if (!string.IsNullOrWhiteSpace(latestMostUpvotes) && latestMostUpvotes != mostUpvotesUpdateMessage)
                {
                    mostUpvotesUpdateMessage = latestMostUpvotes;
                    Console.WriteLine(mostUpvotesUpdateMessage);
                    printedUpdate = true;
                }

                if (printedUpdate) Console.WriteLine(Environment.NewLine);

                Thread.Sleep(DelayBetweenStatusUpdatesInSeconds * 1000);
            }
        }

        private string GetMostProlificString(List<SubredditPost> allPosts)
        {
            var mostProlificAuthor = allPosts
                .GroupBy(ap => ap.AuthorUserId)
                .Select(group => new
                {
                    author = group.Key,
                    count = group.Count()
                })
                .OrderByDescending(a => a.count)
                .FirstOrDefault();

            if (mostProlificAuthor != null && mostProlificAuthor.author != null && !string.IsNullOrWhiteSpace(mostProlificAuthor.author))
            {
                var mostProlificAuthorPost = allPosts.FirstOrDefault(ap => ap.AuthorUserId == mostProlificAuthor.author);
                if (mostProlificAuthorPost != null)
                {
                    return "Author with the most (" + mostProlificAuthor.count + ") posts in subreddit '" + _subreddit + "': [" + mostProlificAuthorPost.AuthorName + "]" + Environment.NewLine;
                }
            }

            return "";
        }

        private string GetMostUpvotesString(List<SubredditPost> allPosts)
        {
            var postWithMostUpvotes = allPosts.OrderByDescending(ap => ap.Upvotes).FirstOrDefault();

            if (postWithMostUpvotes != null)
            {
                return "Post with the most (" + postWithMostUpvotes.Upvotes + ") upvotes in subreddit '" + _subreddit + "': [" + postWithMostUpvotes.Title + "]";
            }

            return "";
        }
    }
}
