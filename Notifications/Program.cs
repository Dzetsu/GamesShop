using Confluent.Kafka;

var conf = new ConsumerConfig
{
    GroupId = "notifications-about-order",
    BootstrapServers = "localhost:9092",
    AutoOffsetReset = AutoOffsetReset.Latest
};

using var consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
consumer.Subscribe("order");

var token = new CancellationTokenSource();
Console.CancelKeyPress += (_, e) =>
{
    e.Cancel = true;
    token.Cancel();
};

try
{
    while (true)
    {
        var consumeResult = consumer.Consume(token.Token);
        Console.WriteLine(consumeResult.Message);
    }
}

catch (OperationCanceledException)
{
}
finally
{
    consumer.Close();
}