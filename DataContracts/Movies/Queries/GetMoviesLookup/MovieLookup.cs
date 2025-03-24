namespace Cinema9.DataContracts.Movies.Queries.GetMoviesLookup;

public record MovieLookup
{
    public required Guid Id { get; init; }
    public required string Title { get; init; }
}
