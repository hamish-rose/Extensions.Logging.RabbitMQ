using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Extensions.Logging.RabbitMQ.IntegrationTests
{
    public class RabbitLoggingHostFixture
    {
        public RabbitLoggingHostFixture()
        {
            Host = new HostBuilder()
                                .ConfigureLogging((ILoggingBuilder logging) =>
                                {
                                    logging.AddConsole();
                                    logging.AddRabbitMQ(config =>
                                    {
                                        config.MinLevel = LogLevel.Debug;
                                    });
                                })
                                .ConfigureServices((context, services) =>
                                {
                                    services.AddSingleton<IMessagingClient, TestingMessagingClient>();
                                })
                                .Build();
        }

        public IHost Host { get; }
    }
}