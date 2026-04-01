using OrderGame.Entities;

namespace OrderGame.Repositories;

public interface IOrderGameRepositiries
{
    Task<OrderedGame?> OrderGame(string nameOfGame, string userName, string dateTime, string idempotent_key, CancellationToken cancellationToken);

    Task<IEnumerable<OrderedGame>> GetAll(string userName, CancellationToken cancellationToken);
    Task<OrderedGame> PaidOrderGame(string userName, long id, CancellationToken cancellationToken);
}