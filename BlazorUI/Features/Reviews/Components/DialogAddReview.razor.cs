using Cinema9.DataContracts.Reviews.Commands.AddReview;
using Microsoft.AspNetCore.Components;

namespace Cinema9.BlazorUI.Features.Reviews.Components;

public partial class DialogAddReview
{
    [Parameter]
    public required Guid MovieId { get; init; }

    [CascadingParameter]
    public required IMudDialogInstance MudDialog { get; init; }

    private AddReviewInput _input = default!;

    protected override void OnParametersSet()
    {
        _input = new()
        {
            MovieId = MovieId,
            ReviewerName = "Name",
            Score = 7,
            Comment = "Bagus juga nih pilem."
        };
    }

    private void Cancel()
    {
        var dialogResult = DialogResult.Ok(false);

        MudDialog.Close(dialogResult);
    }

    private async Task Submit()
    {
        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);
        var restRequest = new RestRequest($"Reviews", Method.Post);
        restRequest.AddBody(_input);

        var restResponse = await restClient.ExecuteAsync<AddReviewOutput>(restRequest);

        if (restResponse.IsSuccessful)
        {
            var dialogResult = DialogResult.Ok(true);

            MudDialog.Close(dialogResult);

            Snackbar.Add($"Review for Movie with ID {MovieId} has been added.", Severity.Success);
        }
        else
        {
            var dialogResult = DialogResult.Ok(false);

            MudDialog.Close(dialogResult);

            Snackbar.Add($"Failed to add Review because {restResponse.ErrorMessage}.", Severity.Error);
        }
    }
}
