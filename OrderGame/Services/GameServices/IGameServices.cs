using OrderGame.Entities;

namespace OrderGame.Services;

public interface IGameServices
{
    Task<IEnumerable<Game>> GetAllGames(CancellationToken cancellationToken);
    Task<Game?> GetGameById(long id, CancellationToken cancellationToken);
    Task<Game?> GetGameByName(string nameOfGame, CancellationToken cancellationToken);
}