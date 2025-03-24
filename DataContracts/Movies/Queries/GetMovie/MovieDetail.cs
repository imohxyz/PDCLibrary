namespace Cinema9.DataContracts.Movies.Queries.GetMovie;

public record MovieDetail
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
    public required string? Synopsis { get; init; }
    public required DateTimeOffset ReleaseDate { get; init; }
    public required float Rating { get; init; }
    public required decimal Budget { get; init; }
}
