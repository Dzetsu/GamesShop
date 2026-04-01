using System.Text.Json.Serialization;
using Confluent.Kafka;
using OrderGame.Entities;
using System.Text.Json;

namespace KafkaProducer;

public class KafkaProducerService
{
    public async Task Message(long id, string userName, string nameOfGame)
    {
        var config = new ProducerConfig()
        {
            BootstrapServers = "localhost:9092"
        };
        
        using var producer = new ProducerBuilder<Null, string>(config).Build();

        var order = new {numoforder = id, username = userName, game = nameOfGame};
        string message = JsonSerializer.Serialize(order);

        await producer.ProduceAsync("order", new Message<Null, string> { Value = message });
        
        Console.WriteLine("Message received!");
    }
}

