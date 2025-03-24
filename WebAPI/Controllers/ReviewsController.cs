using Cinema9.DataContracts.Reviews.Commands.AddReview;
using Cinema9.Logics.Common.Exceptions;
using Cinema9.Logics.Reviews.Commands.AddReview;
using Cinema9.Logics.Reviews.Commands.DeleteReview;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Cinema9.WebAPI.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReviewsController(ISender sender) : ControllerBase
{
    [HttpPost]
    public async Task<ActionResult<AddReviewOutput>> AddReviewAsync([FromBody] AddReviewInput input)
    {
        var request = new AddReviewCommand
        {
            MovieId = input.MovieId,
            ReviewerName = input.ReviewerName,
            Score = input.Score,
            Comment = input.Comment
        };

        var output = await sender.Send(request);

        return output;
    }

    [HttpDelete("{reviewId:guid}")]
    public async Task<ActionResult> DeleteReview(Guid reviewId)
    {
        try
        {
            var request = new DeleteReviewCommand
            {
                ReviewId = reviewId
            };

            await sender.Send(request);
        }
        catch (NotFoundException)
        {
            return NotFound();
        }

        return NoContent();
    }
}
