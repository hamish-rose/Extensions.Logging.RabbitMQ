using System;
using Extensions.Logging.RabbitMQ;
using Microsoft.Extensions.DependencyInjection;
using RabbitMQ.Client;

namespace Microsoft.Extensions.Logging
{
    /// <summary>
    /// RabbitMQ logger extensions
    /// </summary>
    public static class RabbitMQLoggerExtensions
    {
        /// <summary>
        /// Adds RabbitMQ logging to the logging builder, with configuration supplied in the <paramref name="configure"/> action
        /// </summary>
        /// <param name="builder"><see cref="ILoggingBuilder"/> logging builder </param>
        /// <param name="configure"><see cref="Action{RabbitMQLoggerProviderOptions}"/> configuration delegate</param>
        /// <returns><see cref="ILoggingBuilder"/> populated logging builder</returns>
        public static ILoggingBuilder AddRabbitMQ(this ILoggingBuilder builder, Action<RabbitMQLoggerProviderOptions> configure)
        {
            builder.Services.Configure(configure);
            return AddRabbitMQ(builder);
        }

        /// <summary>
        /// Adds RabbitMQ logging to the logging builder. Manually register configuration from another source such as JSON configuration
        /// as <see cref="IOptions{RabbitMQLoggerProviderOptions}"/>
        /// </summary>
        /// <param name="builder"><see cref="ILoggingBuilder"/> builder</param>
        /// <returns><see cref="ILoggingBuilder"/> logging builder</returns>
        public static ILoggingBuilder AddRabbitMQ(this ILoggingBuilder builder)
        {
            RegisterServices(builder.Services);
            return builder;
        }

        /// <summary>
        /// Registers the required services into the dependency injection service collection
        /// </summary>
        /// <param name="services"><see cref="IServiceCollection"/> collection to populate</param>
        private static void RegisterServices(IServiceCollection services)
        {
            services.AddSingleton<IConnectionFactory, ResilientConnectionFactory>();
            services.AddSingleton<IMessagingClient, RabbitMQLoggerClient>();
            services.AddSingleton<ILoggerProvider, RabbitMQLoggerProvider>();
        }
    }
}
