using System;
using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Xunit;

namespace Extensions.Logging.RabbitMQ.IntegrationTests
{
    public class RabbitLoggingHostIntegrationTests : IClassFixture<RabbitLoggingHostFixture>
    {
        public RabbitLoggingHostIntegrationTests(RabbitLoggingHostFixture fixture)
        {
            Host = fixture.Host;
        }

        private IHost Host { get; }

        [Fact]
        public void LogExpectedNumberOfTimesShouldReflectOnRabbit()
        {
            ILogger<RabbitLoggingHostIntegrationTests> logger = Host.Services.GetRequiredService<ILogger<RabbitLoggingHostIntegrationTests>>();
            IMessagingClient client = Host.Services.GetRequiredService<IMessagingClient>();

            int count = 100;

            for (int i = 0; i < count; i++)
            {
                logger.LogInformation("Integration Testing");
            }

            TestingMessagingClient testClient = client as TestingMessagingClient;

            Action a1 = () => testClient.WaitForNumberOfLogs(count);
            a1.Should().NotThrow();
        }
    }
}
