# Extensions.Logging.RabbitMQ

A RabbitMQ logger provider for the Microsoft Extensions Logging framework

## Usage

Add the rabbit logger to your logging builder during host build up.

### ILoggingBuilder

Use AddRabbitMQ() to add the logger provider to the ILoggingBuilder while building the host

```
  new HostBuilder()
     .ConfigureLogging((ILoggingBuilder logging) =>
      {
          logging.AddConsole();
          logging.AddRabbitMQ();
      })
                  ...
```


### Configuration

Register options class in service collection

```
services.Configure<RabbitMQLoggerProviderOptions>(Configuration.GetSection("rabbitLogging"));
```

Add JSON config to appsettings.json

```
"rabbitlogging": {
    "exchange": "myExchange",
    "routingKey": "myRoutingKey",
    "applicationName": "myApplication"
    "minLevel": "Information"
    "rabbitOptions" : {
      "hostName": "localhost",
      "port" : 5762
      "userName": "guest",
      "password": "guest",
      "connectionAttempts": 10
      "connectionAttemptDelay": "00:00:05"
      }
  }
```

Or supply an action when adding to ILoggingBuilder

```
 .ConfigureLogging((ILoggingBuilder logging) =>
  {
      logging.AddRabbitMQ(config =>
      {
          config.MinLevel = LogLevel.Debug;
      });
  })
  ...
```

Calls to logger instances will then be sent as JSON messages to RabbitMQ

```
public MyClass(ILogger<MyClass> logger)
{
  logger.LogInformation("Logging to rabbitMQ!")
}
```
