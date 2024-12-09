
namespace GoldenRaspberryAwards.Application.Models;

public record ProducerAwardsIntervalStats
(
    string Producer, 
    int PreviousWin, 
    int FollowingWin
) : IDto
{
    public int Interval { get; init; } = FollowingWin - PreviousWin;
}
