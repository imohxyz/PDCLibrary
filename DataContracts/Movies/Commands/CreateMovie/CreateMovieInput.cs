namespace Cinema9.DataContracts.Movies.Commands.CreateMovie;

public record CreateMovieInput
{
    public string Title { get; set; } = "The Matrix";
    public DateTimeOffset ReleaseDate { get; set; } = DateTimeOffset.Now;
    public decimal Budget { get; set; } = 50_000_000M;
}
