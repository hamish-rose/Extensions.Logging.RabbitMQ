using System;
using System.Collections.Concurrent;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace Extensions.Logging.RabbitMQ
{
    /// <summary>
    /// Provides RabbitMQ logging capabilities
    /// </summary>
    [ProviderAlias("Rabbit")]
    public class RabbitMQLoggerProvider : ILoggerProvider
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMQLoggerProvider"/> class.
        /// </summary>
        /// <param name="client"><see cref="IMessagingClient"/> messaging client</param>
        /// <param name="options"><see cref="IOptions{RabbitMQLoggerProviderOptions}"/> options</param>
        public RabbitMQLoggerProvider(IMessagingClient client, IOptions<RabbitMQLoggerProviderOptions> options)
        {
            Client = client ?? throw new ArgumentNullException(nameof(client));
            Options = options.Value ?? throw new ArgumentNullException(nameof(options));

            PublishingCancellationToken = new CancellationTokenSource();
            CancellationToken token = PublishingCancellationToken.Token;

            Task.Run(() => PublishLogMessages(token));
        }

        /// <summary>
        /// Gets the provider options
        /// </summary>
        internal RabbitMQLoggerProviderOptions Options { get; }

        /// <summary>
        /// Gets the message queue
        /// </summary>
        internal BlockingCollection<LogMessage> MessageQueue { get; } = new BlockingCollection<LogMessage>();

        /// <summary>
        /// Gets the publishing cancellation token source
        /// </summary>
        private CancellationTokenSource PublishingCancellationToken { get; }

        /// <summary>
        /// Gets the messaging client
        /// </summary>
        private IMessagingClient Client { get; }

        /// <summary>
        /// Creates new <see cref="RabbitMQLogger"/> for the given category
        /// </summary>
        /// <param name="categoryName"><see cref="string"/> category name</param>
        /// <returns><see cref="ILogger"/> logger</returns>
        public ILogger CreateLogger(string categoryName) => new RabbitMQLogger(this, categoryName);

        /// <summary>
        /// Disposes of unmanaged resources
        /// </summary>
        public void Dispose()
        {
            PublishingCancellationToken.Cancel();
        }

        /// <summary>
        /// Writes logs from the collection to the rabbit host
        /// </summary>
        /// <param name="token"><see cref="CancellationToken"/> cancellation token</param>
        private void PublishLogMessages(CancellationToken token)
        {
            try
            {
                while (!token.IsCancellationRequested)
                {
                    LogMessage message = MessageQueue.Take(token);
                    Client.Publish(Options.RoutingKey, message);
                }
            }
            catch (Exception ex) when (ex is ObjectDisposedException || ex is TaskCanceledException)
            {
            }
        }
    }
}
