using Dapper;
using Npgsql;
using OrderGame.Entities;

namespace KafkaProducer;

public class OrderStatusChecker(NpgsqlDataSource dataSource, KafkaProducerService producerService) : BackgroundService
{
    protected override async Task ExecuteAsync(CancellationToken cancellationToken)
    {
        while (!cancellationToken.IsCancellationRequested)
        {
            await using var connection = await dataSource.OpenConnectionAsync(cancellationToken);
            
            while (!cancellationToken.IsCancellationRequested)
            {
                const string sql = @"SELECT ord.id, ord.username, ord.nameofgame
                                     FROM order_games.order ord
                                     JOIN order_games.outbox out ON ord.id = out.order_id
                                     WHERE ord.status = 'paid' AND out.status = 'no send';
                                    ";
                var messageInfo = await connection.QueryFirstOrDefaultAsync<OrderedGame>(sql);

                if (messageInfo == null)
                {
                    continue;
                }
                
                await producerService.Message(messageInfo.id, messageInfo.userName, messageInfo.nameOfGame);
                
                const string updateSql = "update order_games.outbox set status = 'sent' where order_id = @id";
                await connection.ExecuteAsync(updateSql, new { id = messageInfo.id });
                
                await Task.Delay(5000, cancellationToken);
            }
        }
    }
}