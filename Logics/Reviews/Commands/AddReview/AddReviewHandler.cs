using Cinema9.DataContracts.Reviews.Commands.AddReview;
using Cinema9.Domain.Entities;

namespace Cinema9.Logics.Reviews.Commands.AddReview;

public record AddReviewCommand : AddReviewInput, IRequest<AddReviewOutput>
{
}

public class AddReviewHandler(MyDatabase database) : IRequestHandler<AddReviewCommand, AddReviewOutput>
{
    public async Task<AddReviewOutput> Handle(AddReviewCommand request, CancellationToken cancellationToken)
    {
        var movie = await database.Movies
            .Where(x => x.Id == request.MovieId)
            .SingleOrDefaultAsync(cancellationToken)
            ?? throw new NotFoundException("Movie not found");

        var review = new Review
        {
            MovieId = request.MovieId,
            ReviewerName = request.ReviewerName,
            Score = request.Score,
            Comment = request.Comment,
            Created = DateTimeOffset.Now
        };

        await database.Reviews.AddAsync(review, cancellationToken);
        await database.SaveChangesAsync(cancellationToken);

        var averageScore = await database.Reviews
            .AsNoTracking()
            .Where(x => x.MovieId == request.MovieId)
            .Select(x => x.Score)
            .AverageAsync(cancellationToken);

        movie.Rating = Convert.ToSingle(averageScore);
        await database.SaveChangesAsync(cancellationToken);

        var output = new AddReviewOutput
        {
            ReviewId = review.Id
        };

        return output;
    }
}
