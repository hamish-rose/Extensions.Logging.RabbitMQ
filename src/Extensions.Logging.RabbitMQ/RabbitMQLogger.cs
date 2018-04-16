using System;
using System.Text;
using Microsoft.Extensions.Logging;

namespace Extensions.Logging.RabbitMQ
{
    /// <summary>
    /// RabbitMQ logger, enqueues log messages to be logged to RabbitMQ
    /// </summary>
    public class RabbitMQLogger : ILogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMQLogger"/> class.
        /// </summary>
        /// <param name="provider"><see cref="RabbitMQLoggerProvider"/> logger provider</param>
        /// <param name="categoryName"> <see cref="string"/> category name</param>
        public RabbitMQLogger(RabbitMQLoggerProvider provider, string categoryName)
        {
            Provider = provider ?? throw new ArgumentNullException(nameof(provider));
            CategoryName = categoryName ?? throw new ArgumentNullException(nameof(categoryName));
        }

        /// <summary>
        /// Gets the 
        /// </summary>
        private RabbitMQLoggerProvider Provider { get; }

        /// <summary>
        /// Gets the application name publishing logs
        /// </summary>
        private string ApplicationName => Provider.Options.ApplicationName;

        /// <summary>
        /// Gets the minimum level the logger will write
        /// </summary>
        private LogLevel MinLevel => Provider.Options.MinLevel;

        /// <summary>
        /// Gets the category name the logger is used for
        /// </summary>
        private string CategoryName { get; }

        /// <summary>
        /// Begins a logging scope, returns null
        /// </summary>
        /// <typeparam name="TState"> type of state</typeparam>
        /// <param name="state"> log state</param>
        /// <returns><see cref="IDisposable"/> log scope</returns>
        public IDisposable BeginScope<TState>(TState state) => null;

        /// <summary>
        /// Determines whether the given log level is enabled by this logger
        /// </summary>
        /// <param name="logLevel"> the log level to test</param>
        /// <returns> a value indicating whether or not the given log level is enabled by this logger</returns>
        public bool IsEnabled(LogLevel logLevel) => logLevel >= MinLevel;

        /// <summary>
        /// Logs the message to RabbitMQ
        /// </summary>
        /// <typeparam name="TState"> type of log state</typeparam>
        /// <param name="logLevel"> log level</param>
        /// <param name="eventId"> the event ID</param>
        /// <param name="state"> log state</param>
        /// <param name="exception"> exception to be logged</param>
        /// <param name="formatter"> formatter function</param>
        public void Log<TState>(LogLevel logLevel, EventId eventId, TState state, Exception exception, Func<TState, Exception, string> formatter)
        {
            var builder = new StringBuilder();

            builder.Append(DateTimeOffset.Now.ToString("yyyy-MM-dd HH:mm:ss.fff zzz"));
            builder.Append(" [");
            builder.Append(logLevel.ToString());
            builder.Append("] ");
            builder.Append(CategoryName);
            builder.Append(": ");
            builder.AppendLine(formatter(state, exception));

            if (exception != null)
            {
                builder.AppendLine(exception.ToString());
            }

            Provider.MessageQueue.Add(new LogMessage()
            {
                EventId = eventId,
                CategoryName = CategoryName,
                LogLevel = logLevel,
                ApplicationName = ApplicationName,
                Message = builder.ToString(),
                Timestamp = DateTime.UtcNow
            });
        }
    }
}
