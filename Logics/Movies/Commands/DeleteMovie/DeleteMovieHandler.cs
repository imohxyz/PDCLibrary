namespace Cinema9.Logics.Movies.Commands.DeleteMovie;

public record DeleteMovieCommand : IRequest
{
    public required Guid MovieId { get; init; }
}

public class DeleteMovieHandler(MyDatabase database) : IRequestHandler<DeleteMovieCommand>
{
    public async Task Handle(DeleteMovieCommand request, CancellationToken cancellationToken)
    {
        var reviewsCount = await database.Reviews
            .Where(x => x.MovieId == request.MovieId)
            .CountAsync(cancellationToken);

        if (reviewsCount > 0)
        {
            throw new Exception($"Cannot delete Movie with ID {request.MovieId} because it has {reviewsCount} reviews.");
        }

        var movie = await database.Movies
            .Where(x => x.Id == request.MovieId)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Movie not found");

        database.Movies.Remove(movie);
        await database.SaveChangesAsync(cancellationToken);
    }
}
