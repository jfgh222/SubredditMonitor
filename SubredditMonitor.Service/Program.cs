﻿using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using SubredditMonitor.Core.Interfaces;
using SubredditMonitor.Core.Services;
using SubredditMonitor.Infrastructure;
using SubredditMonitor.Infrastructure.Messaging;
using SubredditMonitor.Service;

HostApplicationBuilder builder = Host.CreateApplicationBuilder(args);

builder.Services.AddTransient<IStatusUpdater, StatusUpdater>();
builder.Services.AddTransient<ISubredditPostRetriever, SubredditPostRetriever>();
builder.Services.AddTransient<ISubredditMonitorWorker, SubredditMonitorWorker>();
builder.Services.AddSingleton<ISubredditPostRepository, SubredditPostRepository>();

builder.Services.AddHostedService<SubredditMonitorService>();

using IHost host = builder.Build();

await host.RunAsync();
