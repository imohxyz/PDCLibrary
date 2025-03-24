using Cinema9.Domain.Entities;
using Microsoft.Extensions.Logging;

namespace Cinema9.Infrastructure.Persistence.Seedings;

public class DataSeeder(ILogger<DataSeeder> logger, MyDatabase myDatabase)
{
    private readonly Guid CountryIdUsa = new("e4f98f69-fd32-40b4-9d43-fb072f2c0763");

    public async Task SeedData()
    {
        await SeedCountries();
        await SeedMovies();
    }

    private async Task SeedCountries()
    {
        if (!myDatabase.Countries.Any())
        {
            logger.LogInformation("Seeding data {EntityType}...", "Countries");

            _ = await myDatabase.Countries.AddAsync(new Country
            {
                Code = "IDN",
                Name = "Indonesia"
            });

            _ = await myDatabase.Countries.AddAsync(new Country
            {
                Code = "JPN",
                Name = "Japan"
            });

            _ = await myDatabase.Countries.AddAsync(new Country
            {
                Id = CountryIdUsa,
                Code = "USA",
                Name = "United States of America"
            });

            _ = await myDatabase.Countries.AddAsync(new Country
            {
                Code = "ITA",
                Name = "Italy"
            });

            _ = await myDatabase.SaveChangesAsync();
        }
    }

    private async Task SeedMovies()
    {
        if (!myDatabase.Movies.Any())
        {
            logger.LogInformation("Seeding data {EntityType}...", "Movies");

            _ = await myDatabase.Movies.AddAsync(new Movie
            {
                Title = "The Matrix",
                Synopsis = "A computer hacker learns from mysterious rebels about the true nature of his reality and his role in the war against its controllers.",
                ReleaseDate = new DateTime(1999, 3, 31),
                Rating = 8.7F,
                Budget = 63_000_000M,
                CountryId = CountryIdUsa
            });

            _ = await myDatabase.Movies.AddAsync(new Movie
            {
                Title = "Avengers Endgame",
                Synopsis = "After the devastating events of Avengers: Infinity War (2018), the universe is in ruins. With the help of remaining allies, the Avengers assemble once more in order to reverse Thanos' actions and restore balance to the universe.",
                ReleaseDate = new DateTime(2019, 4, 26),
                Rating = 8.4F,
                Budget = 356_000_000M,
                CountryId = CountryIdUsa
            });

            _ = await myDatabase.Movies.AddAsync(new Movie
            {
                Title = "The Shawshank Redemption",
                Synopsis = "A banker convicted of uxoricide forms a friendship over a quarter century with a hardened convict, while maintaining his innocence and trying to remain hopeful through simple compassion.",
                ReleaseDate = new DateTime(1994, 10, 14),
                Rating = 9.3F,
                Budget = 25_000_000M,
                CountryId = CountryIdUsa
            });

            _ = await myDatabase.SaveChangesAsync();
        }
    }
}
