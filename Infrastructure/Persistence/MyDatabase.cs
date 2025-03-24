using Cinema9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Cinema9.Infrastructure.Persistence;

public class MyDatabase(IConfiguration configuration) : DbContext
{
    private readonly string _connectionString = configuration["ConnectionStrings:MyDatabase"]!;

    public DbSet<Movie> Movies => Set<Movie>();

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlServer(_connectionString);
    }
}
