﻿using System;
using System.Collections.Concurrent;
using System.Threading;

namespace Extensions.Logging.RabbitMQ.IntegrationTests
{
    internal class TestingMessagingClient : IMessagingClient
    {
        internal BlockingCollection<object> Collection { get; } = new BlockingCollection<object>();

        public void Publish(string routingKey, LogMessage message)
        {
            Collection.Add(message);
        }

        internal void WaitForNumberOfLogs(int count)
        {
            int attempts = 5;
            int foundCount = 0;
            TimeSpan delay = TimeSpan.FromMilliseconds(1);
            TimeSpan delayed = default(TimeSpan);

            while (foundCount != count && attempts > 0)
            {
                Thread.Sleep(delay);
                foundCount = Collection.Count;
                attempts--;
                delayed += delay;
            }

            if (foundCount != count)
            {
                throw new InvalidOperationException($"Waited {delayed} for {count} logs, found {foundCount}");
            }
        }
    }
}
