namespace Cinema9.DataContracts.Movies.Commands.CreateMovie;

public record CreateMovieOutput
{
    public required Guid MovieId { get; init; }
}
