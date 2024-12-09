using GoldenRaspberryAwards.Domain.Entities;
using GoldenRaspberryAwards.Domain.Repositories;
using Microsoft.EntityFrameworkCore;

namespace GoldenRaspberryAwards.Infrastructure.EfRepositories;

public sealed class MovieAwardsRepository(AppDbContext context) : IMovieAwardsRepository
{
    public async Task<Movie> AddAsync(Movie movie)
    {
        await context.Movies.AddAsync(movie);
        await context.SaveChangesAsync();
        return movie;
    }

    public async Task<IEnumerable<Movie>> AddRangeAsync(IEnumerable<Movie> movies)
    {
        await context.Movies.AddRangeAsync(movies);
        await context.SaveChangesAsync();
        return movies;
    }

    public async Task<IEnumerable<Movie>> GetAllNomineesAsync()
    {
        return await context.Movies
            .OrderByDescending(m => m.Year)
            .ThenBy(m => m.Title)
            .ToListAsync();
    }

    public async Task<List<Movie>> GetAllNomineesByYearAsync(int year)
    {
        return await context.Movies
            .Where(m => m.Year == year)
            .OrderBy(m => m.Title)
            .ToListAsync();
    }

    public async Task<IEnumerable<Movie>> GetAllWinnersAsync()
    {
        return await context.Movies
            .Where(m => m.IsAwardWinner)
            .ToListAsync();
    }
}
