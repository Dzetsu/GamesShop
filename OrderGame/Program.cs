using KafkaProducer;
using OrderGame.Repositories;
using OrderGame.Services;
using Serilog;
using Serilog.Configuration;
using Serilog.Events;

namespace OrderGame;

public class Program
{
    public static void Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.FromLogContext()
            .WriteTo.BetterStack(
                sourceToken: "MNNvMDdCBEek2x5qX9eoYi6F",
                betterStackEndpoint: "https://s1246737.eu-nbg-2.betterstackdata.com"
                )
            .MinimumLevel.Information()
            .CreateBootstrapLogger();
        
        try
        {
            Log.Information("Starting web application");
            
            var builder = WebApplication.CreateBuilder(args);
            
            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            builder.Services.AddNpgsqlDataSource(builder.Configuration.GetConnectionString("DefaultConnection")!);
            builder.Services.AddSingleton<IGameRepositories, GameRepositories>();
            builder.Services.AddSingleton<IOrderGameRepositiries, OrderGameRepositories>(); 
            builder.Services.AddSingleton<IOrderGameServices, OrderGameServices>();
            builder.Services.AddSingleton<IGameServices, GameServices>();
            builder.Services.AddSingleton<KafkaProducerService>();
            builder.Services.AddHostedService<OrderStatusChecker>();
            builder.Host.UseSerilog();
    
            var app = builder.Build();
            
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }
            
            app.UseHttpsRedirection();

            app.MapControllers();
            
            app.Run();
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application terminated unexpectedly");
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}