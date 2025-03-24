using Cinema9.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace Cinema9.Infrastructure.Persistence;

public class MyDatabase(DbContextOptions<MyDatabase> options) : DbContext(options)
{
    public DbSet<Country> Countries => Set<Country>();
    public DbSet<Movie> Movies => Set<Movie>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        _ = modelBuilder.HasDefaultSchema("cinema9");
        _ = modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());

        base.OnModelCreating(modelBuilder);
    }
}
