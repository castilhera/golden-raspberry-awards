using Dto = GoldenRaspberryAwards.Application.Models;
using Entity = GoldenRaspberryAwards.Domain.Entities;
using GoldenRaspberryAwards.Application.Mappers;
using GoldenRaspberryAwards.Domain.Repositories;
using Microsoft.Extensions.Logging;
using System.Globalization;
using CsvHelper;
using CsvHelper.Configuration;
using GoldenRaspberryAwards.Infrastructure.Mappers;

namespace GoldenRaspberryAwards.Application.Services;

public sealed class MovieAwardsService
(
    IMovieAwardsRepository repository,
    ILogger<MovieAwardsService> logger
) : IMovieAwardsService
{
    private static Entity.Movie[] ReadMoviesCsv(string filePath)
    {
        using var reader = new StreamReader(filePath);

        using var csv = new CsvReader(
            reader,
            new CsvConfiguration(CultureInfo.InvariantCulture)
            {
                Delimiter = ";",
                TrimOptions = TrimOptions.Trim,
                HasHeaderRecord = true,
                IgnoreBlankLines = true
            }
        );

        csv.Context.RegisterClassMap<MovieCsvRowMap>();

        return csv.GetRecords<Entity.Movie>().ToArray();
    }

    public async Task<Dto.Movie> AddAsync(Dto.Movie movie)
    {
        var addedMovie = await repository.AddAsync(movie.ToEntity());

        logger.LogDebug("[Movie] '{movie}' added. (ID {id}).", movie.Title, movie.Id);

        return addedMovie.ToDto();
    }

    public async Task<List<Dto.Movie>> AddRangeAsync(IEnumerable<Dto.Movie> movies)
    {
        var addedMovies = await repository.AddRangeAsync(movies.ToEntityList());

        if (logger.IsEnabled(LogLevel.Debug))
        {
            logger.LogDebug("[Movie] {movieCount} movies added.", addedMovies.Count());

            foreach (var movie in addedMovies)
            {
                logger.LogDebug("[Movie] '{movie}' added. (ID {id}).", movie.Title, movie.Id);
            }
        }

        return addedMovies.ToDTOList();
    }

    public async Task<List<Dto.Movie>> GetAllNomineesAsync()
    {
        var movies = await repository.GetAllNomineesAsync();
        return movies.ToDTOList();
    }

    public async Task<List<Dto.Movie>> GetAllNomineesByYearAsync(int year)
    {
        var movies = await repository.GetAllNomineesByYearAsync(year);
        return movies.ToDTOList();
    }

    public async Task<(Dto.ProducerAwardsIntervalStats[] min, Dto.ProducerAwardsIntervalStats[] max)> GetProducerAwardsIntervalStatsAsync()
    {
        var movies = await repository.GetAllWinnersAsync();

        // group awarded movies by producer and stores the year of each movie
        var producersWithAwardsYears = movies
            .SelectMany(m => m.Producers.Select(p => new { Producer = p, m.Year }))
            .GroupBy(m => m.Producer)
            .Select(g => new
            {
                Producer = g.Key,
                AwardYears = g.Select(m => m.Year).OrderDescending().ToArray()
            })
            .Where(p => p.AwardYears.Length > 1);

        // calculates the interval between each consecutive award
        var producerAwardsInterval = producersWithAwardsYears
            .Select(o => new
            {
                o.Producer,
                AwardsStatistics = o.AwardYears
                            .Zip(
                                    o.AwardYears.Skip(1),
                                    (followingWin, previousWin) => new
                                    {
                                        FollowingWinYear = followingWin,
                                        PreviousWinYear = previousWin,
                                        Interval = followingWin - previousWin
                                    }
                            )
            });

        var allIntervals = producerAwardsInterval.SelectMany(pai => pai.AwardsStatistics.Select(awst => awst.Interval));

        var producersWithMaxInterval = producerAwardsInterval
            .SelectMany(obj => obj.AwardsStatistics
                .Where(d => d.Interval == allIntervals.Max())
                .Select(d => new Dto.ProducerAwardsIntervalStats(obj.Producer, d.PreviousWinYear, d.FollowingWinYear)))
            .ToArray();

        var producersWithMinInterval = producerAwardsInterval
            .SelectMany(obj => obj
                .AwardsStatistics.Where(d => d.Interval == allIntervals.Min())
                .Select(d => new Dto.ProducerAwardsIntervalStats(obj.Producer, d.PreviousWinYear, d.FollowingWinYear)))
            .ToArray();

        return (producersWithMinInterval, producersWithMaxInterval);
    }

    public async Task<List<Dto.Movie>> GetAllWinnersAsync()
    {
        var movies = await repository.GetAllWinnersAsync();
        return movies.ToDTOList();
    }

    public async Task<int> ImportFromCsv(string csvFilePath)
    {
        logger.LogInformation("Seeding Movies data from CSV file '{csvFilePath}'.", csvFilePath);

        if (string.IsNullOrWhiteSpace(csvFilePath) || !File.Exists(csvFilePath))
        {
            logger.LogError("CSV file '{csvFilePath}' not found. Skipping 'Movies' seeding.", csvFilePath);
            return 0;
        }

        var movies = ReadMoviesCsv(csvFilePath);

        logger.LogInformation("{movies} movies found in file.", movies.Length);

        var addedMovies = await repository.AddRangeAsync(movies);

        logger.LogInformation("{movieCount} movies added.", addedMovies.Count());

        logger.LogInformation("Seeding Movies data from CSV file completed.");

        return addedMovies.Count();
    }
}
