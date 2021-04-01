using System;
using Confluent.Kafka;

namespace Kafka.Infra.CrossCutting.Kafka
{
    public interface IKafka
    {
        string SendMessageByKafka(string message);
    }

    public class Kafka : IKafka
    {
        public string SendMessageByKafka(string message)
        {
                Console.WriteLine("Producer inciando.");

            var config = new ProducerConfig { BootstrapServers = Environment.GetEnvironmentVariable("BOOTSTRAP_SERVERS") };

            using (var producer = new ProducerBuilder<Null, string>(config).Build())
            {
                try
                {
                Console.WriteLine("Producer fazendo envio.");
                    var sendResult = producer
                                        .ProduceAsync(Environment.GetEnvironmentVariable("TOPIC"), new Message<Null, string> { Value = (message) })
                                            .GetAwaiter()
                                                .GetResult();


                    Console.WriteLine($"Mensagem enfileirada!");

                    return $"Mensagem '{sendResult.Value}' de '{sendResult.TopicPartitionOffset}'";
                }
                catch (ProduceException<Null, string> e)
                {
                    Console.WriteLine($"Delivery failed: {e.Error.Reason}");
                }
            }

            return string.Empty;
        }

    }
}