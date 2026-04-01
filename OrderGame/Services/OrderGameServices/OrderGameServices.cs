using System.Runtime.InteropServices.JavaScript;
using Dapper;
using KafkaProducer;
using OrderGame.Entities;
using OrderGame.Repositories;
using Npgsql;

namespace OrderGame.Services;

public class OrderGameServices(IOrderGameRepositiries orderRepositories, KafkaProducerService producerService) : IOrderGameServices
{
    public async Task<OrderedGame?> OrderGame(string nameOfGame, string userName, CancellationToken cancellationToken = default)
    {
        var dateTime = DateTime.Now.ToString("f");
        
        var idempotent_key = GenerateIdempotentKey.GenerateKey(userName, nameOfGame);
        
        return await orderRepositories.OrderGame(nameOfGame, userName, dateTime, idempotent_key, cancellationToken);   
    }

    public Task<IEnumerable<OrderedGame>> GetAllOrderedGames(string userName, CancellationToken cancellationToken = default)
    {
        return orderRepositories.GetAll(userName, cancellationToken);
    }

    public Task<OrderedGame> PaidOrderGame(string userName, long id, CancellationToken cancellationToken = default)
    {
        return orderRepositories.PaidOrderGame(userName, id, cancellationToken);
    }
}