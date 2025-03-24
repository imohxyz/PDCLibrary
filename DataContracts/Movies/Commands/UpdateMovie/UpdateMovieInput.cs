namespace Cinema9.DataContracts.Movies.Commands.UpdateMovie;

public record UpdateMovieInput
{
    public required Guid Id { get; init; }
    public required string Title { get; set; }
    public required string? Synopsis { get; set; }
    public required DateTimeOffset ReleaseDate { get; set; }
    public required float Rating { get; set; }
    public required decimal Budget { get; set; }
    public required Guid CountryId { get; set; }
}
