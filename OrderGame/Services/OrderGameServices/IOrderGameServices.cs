using OrderGame.Entities;

namespace OrderGame.Services;

public interface IOrderGameServices
{
    Task<OrderedGame?> OrderGame(string nameOfGame, string userName, CancellationToken cancellationToken);
    Task<IEnumerable<OrderedGame>> GetAllOrderedGames(string userName, CancellationToken cancellationToken);
    Task<OrderedGame> PaidOrderGame(string userName, long id, CancellationToken cancellationToken);
}