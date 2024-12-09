using CsvHelper.Configuration;
using GoldenRaspberryAwards.Domain.Entities;
using System.Text.RegularExpressions;

namespace GoldenRaspberryAwards.Infrastructure.Mappers;

public sealed partial class MovieCsvRowMap : ClassMap<Movie>
{
    public MovieCsvRowMap()
    {
        Map(x => x.Year)
            .Index(0);

        Map(x => x.Title)
            .Index(1);

        Map(x => x.Studios)
            .Index(2)
            .Convert(o => ParseAndConvertToList(o.Row.GetField(2)));

        Map(x => x.Producers)
            .Index(3)
            .Convert(o => ParseAndConvertToList(o.Row.GetField(3)));

        Map(x => x.IsAwardWinner)
            .Index(4)
            .Convert(o => o.Row.GetField(4)?.Equals("yes", StringComparison.OrdinalIgnoreCase) ?? false);
    }

    [GeneratedRegex("(?<!^)(?<=\\S)(?=[A-Z])")]
    private static partial Regex InsertSpaceBeforeCapitalLetters();

    private static string[] ParseAndConvertToList(string? values)
    {
        return values?
                .Replace(", and ", ", ")
                .Replace(" and ", ", ")
                .Split(',')
                .Select(s => InsertSpaceBeforeCapitalLetters().Replace(s.Trim(), " "))
                .ToArray() ?? [];
    }
}
