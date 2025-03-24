namespace Cinema9.Logics.Reviews.Commands.DeleteReview;

public record DeleteReviewCommand : IRequest
{
    public required Guid ReviewId { get; init; }
}

public class DeleteReviewHandler(MyDatabase database) : IRequestHandler<DeleteReviewCommand>
{
    public async Task Handle(DeleteReviewCommand request, CancellationToken cancellationToken)
    {
        var review = await database.Reviews
            .Where(x => x.Id == request.ReviewId)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Review not found");

        var movie = await database.Movies
            .Where(x => x.Id == review.MovieId)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Movie not found");

        database.Reviews.Remove(review);
        await database.SaveChangesAsync(cancellationToken);

        var averageScore = await database.Reviews
            .AsNoTracking()
            .Where(x => x.MovieId == movie.Id)
            .Select(x => x.Score)
            .AverageAsync(cancellationToken);

        movie.Rating = Convert.ToSingle(averageScore);
        await database.SaveChangesAsync(cancellationToken);
    }
}
