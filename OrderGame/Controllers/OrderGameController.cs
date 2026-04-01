using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderGame.Entities;
using OrderGame.Services;
using Serilog;

namespace OrdersGame.Controllers;

[ApiController]
[Route("[controller]")]
public class OrderGameController(IOrderGameServices orderServices, IGameServices gameServices) : ControllerBase
{
    
    [HttpGet]
    public Task<IEnumerable<Game>> GetAllGames()
    {
        Log.Information("Getting all Games");
        return gameServices.GetAllGames(CancellationToken.None);
    }

    [HttpGet]
    [Route("{id:long}")]
    public Task<Game?> GetGameById(long id, CancellationToken cancellationToken)
    {
        Log.Information("Get game by ID");
        return gameServices.GetGameById(id, cancellationToken);
    }

    [HttpGet]
    [Route("nameOfGame")]
    public Task<Game?> GetGameByName(string nameOfGame, CancellationToken cancellationToken)
    {
        Log.Information("Get game by name");
        return gameServices.GetGameByName(nameOfGame, cancellationToken);
    }
    
    [HttpGet]
    [Route("username")]
    public Task<IEnumerable<OrderedGame>> GetAllOrderedGames(string userName, CancellationToken cancellationToken)
    {
        Log.Information("Get ordered games");
        return orderServices.GetAllOrderedGames(userName, cancellationToken);
    }

    [HttpPost]
    public Task<OrderedGame?> OrderGame([FromBody] string nameOfGame, string userName, CancellationToken cancellationToken)
    {
        Log.Information("Order game");
        return orderServices.OrderGame(nameOfGame, userName, cancellationToken);
    }

    [HttpPut]
    [Route("{id:long}")]
    public Task<OrderedGame> PaidOrderGame([FromQuery] string userName, long id, CancellationToken cancellationToken)
    {
        Log.Information("Paid order game");
        return orderServices.PaidOrderGame(userName, id, cancellationToken);
    }
    
}