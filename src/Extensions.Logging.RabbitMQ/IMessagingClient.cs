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
        /// <typeparam name="T"> type of message being published</typeparam>
        /// <param name="routingKey"><see cref="string"/> routing key</param>
        /// <param name="message"><see cref="T"/> message to be publish</param>
        void Publish<T>(string routingKey, T message);
    }
}