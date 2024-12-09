
namespace GoldenRaspberryAwards.Domain.Entities;

public record Movie : IEntity
{
    public int Id { get; set; }
    public int Year { get; set; }
    public string Title { get; set; } = string.Empty;
    public string[] Studios { get; set; } = [];
    public string[] Producers { get; set; } = [];
    public bool IsAwardWinner { get; set; }

    public Movie() { }

    public Movie(int id, int year, string title, string[] studios, string[] producers, bool isAwardWinner)
    {
        Id = id;
        Year = year;
        Title = title;
        Studios = studios;
        Producers = producers;
        IsAwardWinner = isAwardWinner;
    }
}