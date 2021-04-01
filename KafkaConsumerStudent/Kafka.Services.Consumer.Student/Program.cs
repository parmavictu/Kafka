using System;
using System.Threading;
using System.Threading.Tasks;
using Confluent.Kafka;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace Kafka.Services.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Consumer iniciando...");
            var serviceCollection = new ServiceCollection();
            var serviceProvider = serviceCollection.BuildServiceProvider();
            // var _logger = serviceProvider.GetService<ILogger<KafkaMessageHandler>>();
            var conf = new ConsumerConfig
            {
                GroupId = Environment.GetEnvironmentVariable("CONSUMER_GROUP_ID"),
                BootstrapServers = Environment.GetEnvironmentVariable("BOOTSTRAP_SERVERS"),
                AutoOffsetReset = AutoOffsetReset.Earliest
            };

            using (var c = new ConsumerBuilder<Ignore, string>(conf).Build())
            {
                c.Subscribe(Environment.GetEnvironmentVariable("TOPIC"));
                var cts = new CancellationTokenSource();

                Console.WriteLine("Consumer conectado.");

                try
                {
                    Console.WriteLine("Consumer fazendo leitura.");

                    while (true)
                    {
                        var message = c.Consume(cts.Token);
                        // var command = JsonConvert.DeserializeObject<KafkaMessageCommand>(message.Value);
                        // _logger.LogInformation($"Mensagem: {message.Value} recebida de {message.TopicPartitionOffset}");
                        Console.WriteLine($"Mensagem: {message.Value} recebida de {message.TopicPartitionOffset}");

                    }
                }
                catch (OperationCanceledException)
                {
                    c.Close();
                }
            }

        }


    }
}
