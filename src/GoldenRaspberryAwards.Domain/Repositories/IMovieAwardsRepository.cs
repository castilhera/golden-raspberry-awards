using GoldenRaspberryAwards.Domain.Entities;

namespace GoldenRaspberryAwards.Domain.Repositories;

public interface IMovieAwardsRepository
{
    Task<Movie> AddAsync(Movie movie);
    Task<IEnumerable<Movie>> AddRangeAsync(IEnumerable<Movie> movies);
    Task<IEnumerable<Movie>> GetAllNomineesAsync();
    Task<List<Movie>> GetAllNomineesByYearAsync(int year);
    Task<IEnumerable<Movie>> GetAllWinnersAsync();
}
