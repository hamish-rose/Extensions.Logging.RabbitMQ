using System;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using Utf8Json;

namespace Extensions.Logging.RabbitMQ
{
    /// <summary>
    /// RabbitMQ client used to publish log messages to RabbitMQ
    /// </summary>
    public class RabbitMQClient : IMessagingClient
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RabbitMQClient"/> class.
        /// </summary>
        /// <param name="connectionFactory"><see cref="IConnectionFactory"/> rabbitMQ connection factory</param>
        /// <param name="options"><see cref="IOptions{RabbitMQLoggerProviderOptions}"/> options class, configuring the logger provider</param>
        /// <exception cref="ArgumentNullException"> thrown if either constructor parameter is null</exception>
        public RabbitMQClient(IConnectionFactory connectionFactory, IOptions<RabbitMQLoggerProviderOptions> options)
        {
            Options = options.Value ?? throw new ArgumentNullException(nameof(options));

            if (connectionFactory is null)
            {
                throw new ArgumentNullException(nameof(connectionFactory));
            }

            IConnection connection = connectionFactory.CreateConnection();
            Model = connection.CreateModel();
        }

        /// <summary>
        /// Gets the options
        /// </summary>
        private RabbitMQLoggerProviderOptions Options { get; }

        /// <summary>
        /// Gets the rabbit model/channel
        /// </summary>
        private IModel Model { get; }

        /// <summary>
        /// Publishes a message to RabbitMQ, with the specific routing key, via the configured Exchange supplied
        /// in the options.
        /// </summary>
        /// <typeparam name="T"> the type of message being published</typeparam>
        /// <param name="routingKey"> the routing key to use</param>
        /// <param name="message"> the message</param>
        public void Publish<T>(string routingKey, T message)
        {
            Model.BasicPublish(Options.Exchange, routingKey, Model.CreateBasicProperties(), JsonSerializer.Serialize(message));
        }
    }
}
