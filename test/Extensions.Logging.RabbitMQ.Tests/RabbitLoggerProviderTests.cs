using System;
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Xunit;

namespace Extensions.Logging.RabbitMQ.UnitTests
{
    public class RabbitLoggerProviderTests
    {
        [Fact]
        public void ProviderCreatesLoggerSuccessfully()
        {
            var client = new TestingMessagingClient();
            var provider = new RabbitMQLoggerProvider(client, new OptionsWrapper<RabbitMQLoggerProviderOptions>(new RabbitMQLoggerProviderOptions()));

            Action a1 = () => provider.CreateLogger("testing");
            a1.Should().NotThrow();
        }

        [Fact]
        public void LoggersMessagesArePassedToMessagingClient()
        {
            var client = new TestingMessagingClient();
            var provider = new RabbitMQLoggerProvider(client, new OptionsWrapper<RabbitMQLoggerProviderOptions>(new RabbitMQLoggerProviderOptions()));

            ILogger logger = provider.CreateLogger("testing");
            logger.LogInformation("Test log message");

            client.WaitForNumberOfLogs(1);
            client.Collection.Should().HaveCount(1);
        }

        [Fact]
        public void ConfiguredApplicationNameIsIncludedInLogMessage()
        {
            string app = "UnitTests";
            var client = new TestingMessagingClient();
            var provider = new RabbitMQLoggerProvider(client, new OptionsWrapper<RabbitMQLoggerProviderOptions>(new RabbitMQLoggerProviderOptions() { ApplicationName = app }));

            ILogger logger = provider.CreateLogger("testing");
            logger.LogInformation("Test log message");

            client.WaitForNumberOfLogs(1);

            LogMessage message = client.Collection.Single() as LogMessage;
            message.ApplicationName.Should().Be(app);
        }

        [Fact]
        public void LogLevelIsAsExpectedBasedOnMethodUsed()
        {
            var client = new TestingMessagingClient();
            var provider = new RabbitMQLoggerProvider(client, new OptionsWrapper<RabbitMQLoggerProviderOptions>(new RabbitMQLoggerProviderOptions()));

            ILogger logger = provider.CreateLogger("testing");

            logger.LogTrace("Trace logging");
            logger.LogInformation("Information logging");
            logger.LogError("Error logging");

            client.WaitForNumberOfLogs(3);

            IEnumerable<LogMessage> messages = client.Collection.Select(x => x as LogMessage);

            messages.Count(x => x.LogLevel == LogLevel.Information).Should().Be(1);
            messages.Count(x => x.LogLevel == LogLevel.Error).Should().Be(1);
            messages.Count(x => x.LogLevel == LogLevel.Trace).Should().Be(1);
        }

        [Fact]
        public void TimestampIsCloseToNow()
        {
            var client = new TestingMessagingClient();
            var provider = new RabbitMQLoggerProvider(client, new OptionsWrapper<RabbitMQLoggerProviderOptions>(new RabbitMQLoggerProviderOptions()));

            ILogger logger = provider.CreateLogger("testing");
            logger.LogInformation("Test log message");

            client.WaitForNumberOfLogs(1);

            LogMessage message = client.Collection.Single() as LogMessage;
            message.Timestamp.Should().BeCloseTo(DateTime.UtcNow);
        }

        [Fact]
        public void LoggerIsEnabledForLevelsConfigured()
        {
            var client = new TestingMessagingClient();
            var provider = new RabbitMQLoggerProvider(client, new OptionsWrapper<RabbitMQLoggerProviderOptions>(new RabbitMQLoggerProviderOptions() { MinLevel = LogLevel.Warning }));

            ILogger logger = provider.CreateLogger("testing");

            logger.IsEnabled(LogLevel.Information).Should().BeFalse();
            logger.IsEnabled(LogLevel.Warning).Should().BeTrue();
            logger.IsEnabled(LogLevel.Error).Should().BeTrue();
        }
    }
}
