namespace Cinema9.DataContracts.Movies.Commands.CreateMovie;

public record CreateMovieInput
{
    public required string Title { get; set; }
    public required DateTimeOffset ReleaseDate { get; set; }
    public required decimal Budget { get; set; }
    public required Guid CountryId { get; set; }
}
