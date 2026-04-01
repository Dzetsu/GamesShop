namespace OrderGame.Repositories;

public interface IRepositories<T> 
    where T : class
{
    Task<IEnumerable<T>> GetAll(CancellationToken cancellationToken);
    Task<T?> GetById(long id, CancellationToken cancellationToken);
    Task<T?> GetByName(string nameOfGame, CancellationToken cancellationToken);
}