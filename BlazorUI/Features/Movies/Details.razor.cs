using Cinema9.BlazorUI.Features.Movies.Components;
using Cinema9.BlazorUI.Features.Reviews.Components;
using Cinema9.DataContracts.Movies.Queries.GetMovie;
using Microsoft.AspNetCore.Components;

namespace Cinema9.BlazorUI.Features.Movies;

public partial class Details
{
    [Parameter]
    public Guid MovieId { get; set; }

    private MovieDetail _movie = default!;
    private string _errorMessage = string.Empty;

    protected override async Task OnParametersSetAsync()
    {
        await LoadMovie();
    }

    private async Task LoadMovie()
    {
        _errorMessage = string.Empty;

        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);
        var restRequest = new RestRequest($"Movies/{MovieId}", Method.Get);
        var restResponse = await restClient.ExecuteAsync<MovieDetail>(restRequest);

        if (restResponse.Data is null)
        {
            _errorMessage = $"Failed to retrieve Movie with ID {MovieId}.";

            return;
        }

        _movie = restResponse.Data;
    }

    private async Task ShowDialogRemoveMovie()
    {
        _errorMessage = string.Empty;

        var parameters = new DialogParameters
        {
            { nameof(DialogRemoveMovie.MovieId), MovieId },
            { nameof(DialogRemoveMovie.Title), _movie.Title }
        };

        var dialog = await DialogService.ShowAsync<DialogRemoveMovie>("Remove Movie", parameters);
        var dialogResult = await dialog.Result;

        if (dialogResult is null || dialogResult.Data is null)
        {
            _errorMessage = "Cancel remove Movie.";

            return;
        }

        var isConfirmed = (bool)dialogResult.Data;

        if (isConfirmed)
        {
            Navigator.NavigateTo("Movies");
        }
    }

    private async Task ShowDialogAddReview()
    {
        _errorMessage = string.Empty;

        var parameters = new DialogParameters
        {
            { nameof(DialogAddReview.MovieId), MovieId }
        };

        var dialog = await DialogService.ShowAsync<DialogAddReview>("Add Review", parameters);
        var dialogResult = await dialog.Result;

        if (dialogResult is null || dialogResult.Data is null)
        {
            _errorMessage = "Cancel add Review.";

            return;
        }

        var isConfirmed = (bool)dialogResult.Data;

        if (isConfirmed)
        {
            await LoadMovie();
        }
    }

    private async Task ShowDialogRemoveReview(Guid reviewId)
    {
        _errorMessage = string.Empty;

        var parameters = new DialogParameters
        {
            { nameof(DialogRemoveReview.ReviewId), reviewId }
        };

        var dialog = await DialogService.ShowAsync<DialogRemoveReview>("Remove Review", parameters);
        var dialogResult = await dialog.Result;

        if (dialogResult is null || dialogResult.Data is null)
        {
            _errorMessage = "Cancel remove Review.";

            return;
        }

        var isConfirmed = (bool)dialogResult.Data;

        if (isConfirmed)
        {
            await LoadMovie();
        }
    }
}
