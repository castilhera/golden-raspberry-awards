using GoldenRaspberryAwards.Application.Models;

namespace GoldenRaspberryAwards.Api.Models;

public record MovieAwardsYearInfoResponse(int Year, Movie? Winner, List<Movie> Nominees);
