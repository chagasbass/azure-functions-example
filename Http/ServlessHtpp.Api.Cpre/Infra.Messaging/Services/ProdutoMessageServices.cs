using Confluent.Kafka;
using Microsoft.Extensions.Options;
using Serverless.Extensions.Shared.Configurations;
using ServerlessHtpp.Api.Core.Domain.Entities;
using ServerlessHtpp.Api.Core.Infra.Messaging.Bases;
using ServerlessHtpp.Api.Core.Infra.Messaging.Interfaces;

namespace ServerlessHtpp.Api.Core.Infra.Messaging.Services;

public class ProdutoMessageServices : BaseKafkaMessageServices, IProdutoMessageServices
{
    private readonly MessagingConfigurationOptions _options;

    public ProdutoMessageServices(IOptions<MessagingConfigurationOptions> options) : base(options)
    {
        _options = options.Value;
    }

    public async Task GerarEventoDeProdutoAtualizadoAsync(Produto produto)
    {
        var producerConfig = GenerateProduceConfigForSendingMessages();

        using var producer = new ProducerBuilder<Null, string>(producerConfig).Build();

        var message = SerializeMessage(produto);

        await producer.ProduceAsync(_options.Topic, new Message<Null, string>
        {
            Value = message
        });
    }
}
