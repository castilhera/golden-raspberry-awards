using GoldenRaspberryAwards.Application.Models;

namespace GoldenRaspberryAwards.Api.Models;

public record ProducerAwardsIntervalStatsResponse
(
    ProducerAwardsIntervalStats[] Min, 
    ProducerAwardsIntervalStats[] Max
);
