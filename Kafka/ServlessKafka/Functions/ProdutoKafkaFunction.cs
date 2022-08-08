using Microsoft.Azure.Functions.Worker;
using Newtonsoft.Json.Linq;
using Serverless.Extensions.Shared.Logs.Services;
using ServlessKafka.Core.Domain.Entities;
using System.Text.Json;

namespace ServlessKafka.Functions;

public class ProdutoKafkaFunction
{
    private readonly ILogServices _logServices;

    public ProdutoKafkaFunction(ILogServices logServices)
    {
        _logServices = logServices;
    }

    [Function("ProdutoKafkaFunction")]
    public async Task RunAsync([KafkaTrigger(
        "KafkaConnection", "ProductTopic",ConsumerGroup = "ConsumerGroup",  IsBatched = true)]
         string[]  kafkaEvents, FunctionContext context)
    {
        var produtos = new List<Produto>();

        foreach (var kafkaEvent in kafkaEvents)
        {
            var dadosRecuperados = JObject.Parse(kafkaEvent)["Value"].ToString();

            var jsonConfiguration = new JsonSerializerOptions()
            {
                PropertyNameCaseInsensitive = true
            };

            _logServices.LogData.AddRequestBody(dadosRecuperados);

            produtos.Add(JsonSerializer.Deserialize<Produto>(dadosRecuperados, jsonConfiguration));

            _logServices.WriteLog();
        }
    }
}
