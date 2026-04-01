using Dapper;
using Npgsql;
using OrderGame.Entities;

namespace OrderGame.Repositories;

public class GameRepositories(NpgsqlDataSource dataSource) : IGameRepositories
{
    public async Task<IEnumerable<Game>> GetAll(CancellationToken cancellationToken = default)
    {
        var connection = await dataSource.OpenConnectionAsync(cancellationToken);

        const string sql = "select * from order_games.games";
        
        return await connection.QueryAsync<Game>(sql);
    }

    public async Task<Game?> GetById(long id, CancellationToken cancellationToken = default)
    {
        var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        
        const string sql = "select * from order_games.games where id = @id";
        
        return await connection.QuerySingleOrDefaultAsync<Game>(sql, new { id });
    }

    public async Task<Game?> GetByName(string nameOfGame, CancellationToken cancellationToken = default)
    {
        var connection = await dataSource.OpenConnectionAsync(cancellationToken);
        
        const string sql = "select * from order_games.games where nameOfGame = @nameOfGame";
        
        return await connection.QuerySingleOrDefaultAsync<Game>(sql, new { nameOfGame });
    }
}