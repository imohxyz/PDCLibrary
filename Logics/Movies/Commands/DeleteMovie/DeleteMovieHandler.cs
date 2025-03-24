namespace Cinema9.Logics.Movies.Commands.DeleteMovie;

public record DeleteMovieCommand : IRequest
{
    public required Guid MovieId { get; init; }
}

public class DeleteMovieHandler(MyDatabase database) : IRequestHandler<DeleteMovieCommand>
{
    public async Task Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var movie = await database.Movies
            .Where(x => x.Id == request.MovieId)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Movie not found");

        database.Movies.Remove(movie);
        await database.SaveChangesAsync(cancellationToken);
    }
}
