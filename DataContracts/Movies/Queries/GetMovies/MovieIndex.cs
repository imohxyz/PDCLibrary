namespace Cinema9.DataContracts.Movies.Queries.GetMovies;

public record MovieIndex
{
    public required Guid Id { get; init; }
    public required string Title { get; init; } = string.Empty;
    public required float Rating { get; init; }
}
