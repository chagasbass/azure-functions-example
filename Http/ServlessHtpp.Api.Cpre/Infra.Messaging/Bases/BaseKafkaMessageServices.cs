using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Serverless.Extensions.Shared.Configurations;
using System.Text.Json;

namespace ServerlessHtpp.Api.Core.Infra.Messaging.Bases;

public abstract class BaseKafkaMessageServices
{
    private readonly MessagingConfigurationOptions _options;

    protected BaseKafkaMessageServices(IOptions<MessagingConfigurationOptions> options)
    {
        _options = options.Value;
    }

    public string SerializeMessage(object message) => JsonSerializer.Serialize(message);

    public ProducerConfig GenerateProduceConfigForSendingMessages()
    {
        var producerConfig = new ProducerConfig
        {
            BootstrapServers = _options.BootstrapServer
        };

        return producerConfig;
    }
}
