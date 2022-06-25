﻿using System;
using System.Threading;
using System.Diagnostics;

using nanoFramework.Hosting;

namespace Hosting
{
    internal class PublisherService : BackgroundService
    {
        private readonly Random _random;
        private readonly BackgroundQueue _queue;

        public PublisherService(BackgroundQueue queue)
        {
            _queue = queue;
            _random = new Random();
        }

        protected override void ExecuteAsync(CancellationToken cancellationToken)
        {
            Debug.WriteLine($"Service '{nameof(PublisherService)}' is now running in the background.");
            cancellationToken.Register(() => Debug.WriteLine($"Service '{nameof(PublisherService)}' is stopping."));

            while (!cancellationToken.IsCancellationRequested)
            {
                try
                {
                    Thread.Sleep(1);
                    var workItem = FakeSensor();
                    _queue.Enqueue(workItem);
                }
                catch (Exception ex)
                {
                    Debug.WriteLine($"An error occurred when enqueueing work item. Exception: {ex}");
                }
            }
        }

        private int FakeSensor()
        {
            Thread.Sleep(_random.Next(100));
            return _random.Next(1000);
        }
    }
}