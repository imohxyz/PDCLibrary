using Microsoft.AspNetCore.Components;

namespace Cinema9.BlazorUI.Features.Movies.Components;

public partial class DialogRemoveMovie
{
    [Parameter]
    public required Guid MovieId { get; init; }

    [Parameter]
    public required string Title { get; init; }

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
        var restRequest = new RestRequest($"Movies/{MovieId}", Method.Delete);
        var restResponse = await restClient.ExecuteAsync(restRequest);

        if (restResponse.IsSuccessful)
        {
            var dialogResult = DialogResult.Ok(true);

            MudDialog.Close(dialogResult);

            Snackbar.Add($"Movie {Title} has been removed.", Severity.Success);
        }
        else
        {
            var dialogResult = DialogResult.Ok(false);

            MudDialog.Close(dialogResult);

            Snackbar.Add($"Failed to remove movie {Title} because {restResponse.ErrorMessage}.", Severity.Error);
        }
    }
}
