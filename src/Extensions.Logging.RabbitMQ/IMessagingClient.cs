namespace Extensions.Logging.RabbitMQ
{
    /// <summary>
    /// Messaging client interface
    /// </summary>
    public interface IMessagingClient
    {
        /// <summary>
        /// Publish a message
        /// </summary>
        /// <param name="routingKey"><see cref="string"/> routing key</param>
        /// <param name="message"><see cref="LogMessage"/> message to be publish</param>
        void Publish(string routingKey, LogMessage message);
    }
}