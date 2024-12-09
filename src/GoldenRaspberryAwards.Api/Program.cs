using GoldenRaspberryAwards.Api.Endpoints;
using GoldenRaspberryAwards.Application.Services;
using GoldenRaspberryAwards.Domain.Repositories;
using GoldenRaspberryAwards.Infrastructure.EfRepositories;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace GoldenRaspberryAwards.Api;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Logging
            .ClearProviders()
            .AddConsole();

        builder.Services
            .AddEndpointsApiExplorer()
            .AddSwaggerGen();

        builder.Services
            .AddDbContext<AppDbContext>(options =>
                options
                    .UseInMemoryDatabase("GoldenRaspberryAwardsDb")
                    .UseQueryTrackingBehavior(QueryTrackingBehavior.NoTracking)
            );

        builder.Services
            .AddScoped<IMovieAwardsRepository, MovieAwardsRepository>()
            .AddScoped<IMovieAwardsService, MovieAwardsService>();

        var app = builder.Build();

        if (app.Environment.IsDevelopment())
        {
            app
                .UseSwagger()
                .UseSwaggerUI()
                .UseDeveloperExceptionPage();
        }

        DatabaseSeed(app);

        app.UseHttpsRedirection();

        app.MapAwardsEndpoints();

        app.Run();
    }

    private static void DatabaseSeed(WebApplication app)
    {
        using var scope = app.Services.CreateScope();

        var movieAwardsService = scope.ServiceProvider.GetRequiredService<IMovieAwardsService>();

        string csvFilePath = Path.Combine(Environment.CurrentDirectory, "Data", "movielist.csv");

        var _ = movieAwardsService.ImportFromCsv(csvFilePath);
    }
}
