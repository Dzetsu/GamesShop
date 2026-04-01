using System.ComponentModel.Design;
using Confluent.Kafka;
using Telegram.Bot;
using Telegram.Bot.Types.Enums;
using System.Text.Json;

class Program
{
    [Obsolete("Obsolete")]
    static async Task Main()
    {
        var conf = new ConsumerConfig
        {
            GroupId = "order",
            BootstrapServers = "localhost:9092",
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        using var consumer = new ConsumerBuilder<Ignore, string>(conf).Build();
        consumer.Subscribe("order");

        var saverOrderId = new HashSet<long>();
        
        try
        {
            while (true)
            {
                var consumeResult = consumer.Consume(CancellationToken.None);
                var order = JsonSerializer.Deserialize<JsonElement>(consumeResult.Value);

                if (!order.TryGetProperty("numoforder", out var orderIdProperty) ||
                    !order.TryGetProperty("username", out var userNameProperty) ||
                    !order.TryGetProperty("game", out var gameProperty))
                {
                    Console.WriteLine("Missing required properties in order message");
                    continue;
                }
                
                long orderId = order.GetProperty("numoforder").GetInt64(); 
                
                if (saverOrderId.Contains(orderId)) 
                {
                    Console.WriteLine($"Order {orderId} already processed.");
                    continue;
                }
                
                string botToken = "7603428024:AAEKsVb_pNvAJ3KGqBW3w8aJxxaikZVqzLI";
                string chatId = "1620966794";

                var botClient = new TelegramBotClient(botToken);

                string messageText =
                    $"Пользователь {order.GetProperty("username")} оплатил заказ за игру {order.GetProperty("game")}";
                
                try
                {
                    var message = await botClient.SendMessage(
                        chatId: chatId,
                        text: messageText,
                        parseMode: ParseMode.Markdown
                    );

                    Console.WriteLine($"Message received! ID: {message.MessageId}");
                    
                    saverOrderId.Add(orderId);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Error! ID: {ex.Message}");
                }
            }
        }
        catch
        {
            //ya skipnul
        }
        finally
        {
            consumer.Close();
        }
    }
}