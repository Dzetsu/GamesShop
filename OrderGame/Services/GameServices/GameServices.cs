using OrderGame.Entities;
using OrderGame.Repositories;

namespace OrderGame.Services;

public class GameServices(IGameRepositories gameRepositories) : IGameServices
{
    public Task<IEnumerable<Game>> GetAllGames(CancellationToken cancellationToken = default)
    {
        return gameRepositories.GetAll(cancellationToken);
    }

    public Task<Game?> GetGameById(long id, CancellationToken cancellationToken = default)
    {
        return gameRepositories.GetById(id, cancellationToken);
    }

    public Task<Game?> GetGameByName(string nameOfGame, CancellationToken cancellationToken = default)
    {
        return gameRepositories.GetByName(nameOfGame, cancellationToken);
    }
}