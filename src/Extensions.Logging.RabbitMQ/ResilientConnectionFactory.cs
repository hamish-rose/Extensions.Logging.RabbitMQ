using System;
using System.Threading;
using Microsoft.Extensions.Options;
using RabbitMQ.Client;
using RabbitMQ.Client.Exceptions;

namespace Extensions.Logging.RabbitMQ
{
    /// <summary>
    /// Rabbit MQ connection factory, configurable retry attempts
    /// </summary>
    internal class ResilientConnectionFactory : ConnectionFactory
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ResilientConnectionFactory"/> class.
        /// </summary>
        /// <param name="options"><see cref="IOptions{RabbitMQLoggerProviderOptions}"/> options</param>
        public ResilientConnectionFactory(IOptions<RabbitMQLoggerProviderOptions> options)
        {
            RabbitMQClientOptions rabbitOptions = options.Value.RabbitOptions;

            DispatchConsumersAsync = true;
            HostName = rabbitOptions.HostName;
            Port = rabbitOptions.Port;
            UserName = rabbitOptions.UserName;
            Password = rabbitOptions.Password;

            Ssl = new SslOption()
            {
                Enabled = rabbitOptions.IsSsl,
                ServerName = rabbitOptions.SslServer
            };
        }

        /// <summary>
        /// Gets the number of attempts to make connect to rabbit
        /// </summary>
        private int ConnectionAttempts { get; }

        /// <summary>
        /// Gets the length of time to delay between connection attempts
        /// </summary>
        private TimeSpan ConnectionAttemptDelay { get; }

        /// <summary>
        /// Attempts to open a connection to the rabbit host. If <see cref="BrokerUnreachableException"/> is thrown while
        /// attempting to connect, after the configured delay period another attempt will be made until all attempts have been exhausted.
        /// </summary>
        /// <exception cref="InvalidOperationException"> thrown if a connection to RabbitMQ could not be established</exception>
        /// <returns><see cref="IConnection"/> Open connection to the rabbit host</returns>
        public override IConnection CreateConnection()
        {
            int remaningAttempts = 10;

            while (remaningAttempts > 0)
            {
                try
                {
                    return base.CreateConnection();
                }
                catch (BrokerUnreachableException)
                {
                    remaningAttempts--;
                    Thread.Sleep(5000);
                }
            }

            throw new InvalidOperationException("Rabbit connection could not be established");
        }
    }
}
