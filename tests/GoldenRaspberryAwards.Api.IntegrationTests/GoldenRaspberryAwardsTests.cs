using CsvHelper;
using GoldenRaspberryAwards.Api.Models;
using GoldenRaspberryAwards.Application.Models;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.VisualStudio.TestPlatform.TestHost;
using System.Net.Http.Json;

namespace GoldenRaspberryAwards.Api.IntegrationTests;

public class GoldenRaspberryAwardsTests(
    WebApplicationFactory<Program> factory
) : IClassFixture<WebApplicationFactory<Program>>
{
    private readonly HttpClient _client = factory.CreateClient();

    private const string ProducersAwardsIntervalUrl = "/awards/producers-awards-interval";

    [Fact]
    public async Task GetProducerConsecutiveAwards_ReturnsCorrectData()
    {
        var expectedResult = new ProducerAwardsIntervalStatsResponse(
            [new ProducerAwardsIntervalStats("Joel Silver", 1990, 1991)],
            [new ProducerAwardsIntervalStats("Matthew Vaughn", 2002, 2015)]
        );

        var response = await _client.GetFromJsonAsync<ProducerAwardsIntervalStatsResponse>(ProducersAwardsIntervalUrl);

        Assert.NotNull(response);
        Assert.Equal(response.Min[0].Interval, expectedResult.Min[0].Interval);
        Assert.Equal(response.Max[0].Interval, expectedResult.Max[0].Interval);
    }
}
