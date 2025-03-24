namespace Cinema9.Domain.Entities;

public class Movie
{
    public Guid Id { get; init; } = Guid.NewGuid();
    public required string Title { get; set; }
    public required string? Synopsis { get; set; }
    public required DateTimeOffset ReleaseDate { get; set; }
    public required float Rating { get; set; }
    public required decimal Budget { get; set; }

    public required Guid CountryId { get; set; }
    public Country Country { get; set; } = default!;

    public ICollection<Review> Reviews { get; set; } = [];
}
