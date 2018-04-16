using System;
using Microsoft.Extensions.Logging;

namespace Extensions.Logging.RabbitMQ
{
    /// <summary>
    /// Models a log message, published to RabbitMQ
    /// </summary>
    public class LogMessage
    {
        /// <summary>
        /// Gets or sets the event ID
        /// </summary>
        public EventId EventId { get; set; }

        /// <summary>
        /// Gets or sets the category name the log was published from
        /// </summary>
        public string CategoryName { get; set; }

        /// <summary>
        /// Gets or sets the log message
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the application name the log is from
        /// </summary>
        public string ApplicationName { get; set; }

        /// <summary>
        /// Gets or sets the Log Level
        /// </summary>
        public LogLevel LogLevel { get; set; }

        /// <summary>
        /// Gets or sets the UTC timestamp of the message 
        /// </summary>
        public DateTime Timestamp { get; set; }
    }
}