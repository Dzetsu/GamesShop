using System.Data;
using System.Reflection;
using System.Transactions;
using Npgsql;
using Dapper;
using OrderGame.Entities;

namespace OrderGame.Repositories;

public class OrderGameRepositories(NpgsqlDataSource dataSource) : IOrderGameRepositiries
{
    public async Task<OrderedGame?> OrderGame(string nameOfGame, string userName, string dateTime, string idempotent_key, CancellationToken cancellationToken = default)
    {
        var connection = await dataSource.OpenConnectionAsync(cancellationToken);

        var exeistingOrder = await connection.ExecuteScalarAsync<int>(
            "select count(1) from order_games.order where idempotent_key = @idempotent_key",
            new { idempotent_key });

        if (exeistingOrder > 0)
        {
            const string sql = "select id, username, datetime, nameofgame, status from order_games.order where idempotent_key = @idempotent_key";
            
            return await connection.QueryFirstOrDefaultAsync<OrderedGame>(sql, new { idempotent_key });
        }
        
        const string sql2 = "select qty from order_games.games where nameofgame = @nameOfGame;";
        
        var qty = await connection.QueryFirstOrDefaultAsync<long>(sql2, new { nameOfGame });
        
        if (qty > 0)
        {
            var transaction = await connection.BeginTransactionAsync(cancellationToken);

            const string sql = "update order_games.games set qty = qty - 1 where nameofgame = @nameOfGame;" +
                               "insert into order_games.order (username, datetime, nameofgame, idempotent_key) values (@userName,  @dateTime, @nameOfGame, @idempotent_key);" +
                               "select id, username, datetime, nameofgame, status from order_games.order where (username = @userName) and (datetime = @dateTime);";
            
            var res = await connection.QueryFirstOrDefaultAsync<OrderedGame>(sql, new { userName, dateTime, nameOfGame, idempotent_key }, transaction);
            
            await transaction.CommitAsync(cancellationToken);

            return res;
        }
        else
            throw new InvalidCastException("Нет в наличии!");
    }
    
    public async Task<IEnumerable<OrderedGame>> GetAll(string userName, CancellationToken cancellationToken = default)
    {
        var connection = await dataSource.OpenConnectionAsync(cancellationToken);

        const string sql = "select id, username, datetime, nameofgame, status from order_games.order where username = @userName;";
        
        return await connection.QueryAsync<OrderedGame>(sql, new { userName });
    }

    public async Task<OrderedGame> PaidOrderGame(string userName, long id, CancellationToken cancellationToken = default)
    {
        var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        
        var transaction = await connection.BeginTransactionAsync(cancellationToken);
        
        const string sql = "update order_games.order set status = 'paid' where (username, id) = (@userName, @id);" +
                           "select id, username, datetime, nameofgame, status from order_games.order where (username, id) = (@userName, @id);" +
                           "insert into order_games.outbox (order_id, status) values (@id, 'no send')";
        
        var res = await connection.QueryFirstAsync<OrderedGame>(sql, new { userName, id }, transaction);
        
        await transaction.CommitAsync(cancellationToken);
        
        return res;
    }
}

