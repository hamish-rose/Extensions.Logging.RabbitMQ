using System;

namespace Extensions.Logging.RabbitMQ
{
    /// <summary>
    /// Options class for configuring rabbitMQ related connection options
    /// </summary>
    public class RabbitMQClientOptions
    {
        /// <summary>
        /// Gets or sets the rabbit MQ host name, default value of localhost
        /// </summary>
        public string HostName { get; set; } = "localhost";

        /// <summary>
        /// Gets or sets the port to use when connecting to the rabbit host, default value 5672.
        /// </summary>
        public int Port { get; set; } = 5672;

        /// <summary>
        /// Gets or sets the username to use when connecting to Rabbit, default value guest
        /// </summary>
        public string UserName { get; set; } = "guest";

        /// <summary>
        /// Gets or sets the password to use when connecting to Rabbit, default value guest
        /// </summary>
        public string Password { get; set; } = "guest";

        /// <summary>
        /// Gets or sets the connection attempts value, the number of times to attempt to initially connect
        /// </summary>
        public int ConnectionAttempts { get; set; } = 10;

        /// <summary>
        /// Gets or sets the connection attempt delay, the amount of time to delay between connection attempts
        /// </summary>
        public TimeSpan ConnectionAttemptDelay { get; set; } = new TimeSpan(0, 0, 6);

        /// <summary>
        /// Gets or sets a value indicating whether the rabbit host is using SSL
        /// </summary>
        public bool IsSsl { get; set; } = false;

        /// <summary>
        /// Gets or sets the SSL server name
        /// </summary>
        public string SslServer { get; set; }
    }
}
