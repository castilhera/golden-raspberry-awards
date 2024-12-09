using Dto = GoldenRaspberryAwards.Application.Models;
using Entity = GoldenRaspberryAwards.Domain.Entities;

namespace GoldenRaspberryAwards.Application.Mappers;

public static class MovieMapperExtensions
{
    public static Dto.Movie ToDto(this Entity.Movie movie) 
        => new(
            movie.Id,
            movie.Year,
            movie.Title,
            movie.Studios,
            movie.Producers,
            movie.IsAwardWinner
        );

    public static Entity.Movie ToEntity(this Dto.Movie movie) 
        => new(
            movie.Id,
            movie.Year,
            movie.Title,
            movie.Studios,
            movie.Producers,
            movie.IsAwardWinner
        );

    public static List<Dto.Movie> ToDTOList(this IEnumerable<Entity.Movie> movies) 
        => movies.Select(m => m.ToDto()).ToList();

    public static List<Entity.Movie> ToEntityList(this IEnumerable<Dto.Movie> movies)
        => movies.Select(m => m.ToEntity()).ToList();
}
