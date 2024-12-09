using GoldenRaspberryAwards.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoldenRaspberryAwards.Infrastructure.EfRepositories;

public sealed class AppDbContext(DbContextOptions options) : DbContext(options)
{
    public DbSet<Movie> Movies { get; set; }
}
