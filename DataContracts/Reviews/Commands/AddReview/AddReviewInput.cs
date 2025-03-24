namespace Cinema9.DataContracts.Reviews.Commands.AddReview;

public record AddReviewInput
{
    public required Guid MovieId { get; init; }
    public required string ReviewerName { get; set; }
    public required int Score { get; set; }
    public required string Comment { get; set; }
}
