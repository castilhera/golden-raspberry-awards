namespace GoldenRaspberryAwards.Application.Models;

public record Movie
(
    int Id, 
    int Year, 
    string Title, 
    string[] Studios, 
    string[] Producers, 
    bool IsAwardWinner
) : IDto;
