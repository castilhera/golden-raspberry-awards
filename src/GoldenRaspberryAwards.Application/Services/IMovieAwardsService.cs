using GoldenRaspberryAwards.Application.Models;

namespace GoldenRaspberryAwards.Application.Services;

public interface IMovieAwardsService
{
    Task<Movie> AddAsync(Movie movie);
    Task<List<Movie>> AddRangeAsync(IEnumerable<Movie> movies);
    Task<List<Movie>> GetAllNomineesAsync();
    Task<List<Movie>> GetAllNomineesByYearAsync(int year);
    Task<(ProducerAwardsIntervalStats[] min, ProducerAwardsIntervalStats[] max)> GetProducerAwardsIntervalStatsAsync();
    Task<List<Movie>> GetAllWinnersAsync();
    Task<int> ImportFromCsv(string csvFilePath);
}
