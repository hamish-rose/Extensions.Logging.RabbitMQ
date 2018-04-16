using Microsoft.Extensions.Logging;

namespace Extensions.Logging.RabbitMQ
{
    /// <summary>
    /// Options class for configuring the logger provider
    /// </summary>
    public class RabbitMQLoggerProviderOptions
    {
        /// <summary>
        /// Gets or sets the rabbit options
        /// </summary>
        public RabbitMQClientOptions RabbitOptions { get; set; } = new RabbitMQClientOptions();

        /// <summary>
        /// Gets or sets the exchange to use when publishing log messages
        /// </summary>
        public string Exchange { get; set; }

        /// <summary>
        /// Gets or sets the routing key to use when publishing log messages
        /// </summary>
        public string RoutingKey { get; set; }

        /// <summary>
        /// Gets or sets the application name which published the log message, included as a property on the rabbitMQ message and as a property
        /// on the log message body
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the minimum log level that should be published to rabbitMQ
        /// </summary>
        public LogLevel MinLevel { get; set; } = LogLevel.Information;
    }
}