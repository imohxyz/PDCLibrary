using Microsoft.AspNetCore.Components;

namespace Cinema9.BlazorUI.Features.Reviews.Components;

public partial class DialogRemoveReview
{
    [Parameter]
    public required Guid ReviewId { get; init; }

    [CascadingParameter]
    public required IMudDialogInstance MudDialog { get; init; }

    private void Cancel()
    {
        var dialogResult = DialogResult.Ok(false);

        MudDialog.Close(dialogResult);
    }

    private async Task Submit()
    {
        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);
        var restRequest = new RestRequest($"Reviews/{ReviewId}", Method.Delete);
        var restResponse = await restClient.ExecuteAsync(restRequest);

        if (restResponse.IsSuccessful)
        {
            var dialogResult = DialogResult.Ok(true);

            MudDialog.Close(dialogResult);

            Snackbar.Add($"Review with ID {ReviewId} has been removed.", Severity.Success);
        }
        else
        {
            var dialogResult = DialogResult.Ok(false);

            MudDialog.Close(dialogResult);

            Snackbar.Add($"Failed to remove Review with ID {ReviewId} because {restResponse.ErrorMessage}.", Severity.Error);
        }
    }
}
