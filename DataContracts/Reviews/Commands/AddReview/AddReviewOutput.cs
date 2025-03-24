namespace Cinema9.DataContracts.Reviews.Commands.AddReview;

public record AddReviewOutput
{
    public required Guid ReviewId { get; init; }
}
