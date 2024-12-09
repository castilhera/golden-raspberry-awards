
namespace GoldenRaspberryAwards.Application.Models;

public record MovieAwardsYearInfo(int Year, Movie Winner, Movie[] Nominees) : IDto;
