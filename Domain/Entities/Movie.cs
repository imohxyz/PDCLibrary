namespace Cinema9.Domain.Entities;

public class Movie
{
    public Guid Id { get; init; }
    public required string Title { get; set; }
    public required string? Synopsis { get; set; }
    public required DateTimeOffset ReleaseDate { get; set; }
    public required float Rating { get; set; }
    public required decimal Budget { get; set; }
}
