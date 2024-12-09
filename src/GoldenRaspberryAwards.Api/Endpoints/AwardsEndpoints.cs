using GoldenRaspberryAwards.Api.Models;
using GoldenRaspberryAwards.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace GoldenRaspberryAwards.Api.Endpoints;

public static class AwardsEndpoints
{
    public static void MapAwardsEndpoints(this IEndpointRouteBuilder app)
    {
        var route = app.MapGroup("awards");

        route.MapGet("/{year}", GetAwardsInfoByYear);
        route.MapGet("producers-awards-interval", GetProducerAwardsInterval);
    }

    [Tags("Awards")]
    [EndpointSummary("Gets the award nominees and winner of the given year.")]
    private static async Task<IResult> GetAwardsInfoByYear(
        [FromRoute] int year,
        IMovieAwardsService movieAwardsService
    )
    {
        var nominees = await movieAwardsService.GetAllNomineesByYearAsync(year);
        var winner = nominees.FirstOrDefault(n => n.IsAwardWinner);

        return TypedResults.Ok(new MovieAwardsYearInfoResponse(year, winner, nominees));
    }

    [Tags("Awards")]
    [EndpointSummary("Gets the producers with minimum and maximum interval between consecutive awards.")]
    private static async Task<IResult> GetProducerAwardsInterval(
        IMovieAwardsService awardsServices
    )
    {
        (var min, var max) = await awardsServices.GetProducerAwardsIntervalStatsAsync();

        var response = new ProducerAwardsIntervalStatsResponse(min, max);

        return TypedResults.Ok(response);
    }
}
