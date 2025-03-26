using Cinema9.DataContracts.Rents.Queries.GetRent;
using Microsoft.AspNetCore.Components;

namespace Cinema9.BlazorUI.Features.Rents;

public partial class Details
{
    [Parameter]
    public Guid RentId { get; set; }

    private RentDetail _rent = default!;
    private string _errorMessage = string.Empty;

    protected override async Task OnParametersSetAsync()
    {
        await LoadRent();
    }

    private async Task LoadRent()
    {
        _errorMessage = string.Empty;

        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);
        var restRequest = new RestRequest($"Rents/{RentId}", Method.Get);
        var restResponse = await restClient.ExecuteAsync<RentDetail>(restRequest);

        if (restResponse.Data is null)
        {
            _errorMessage = $"Failed to retrieve Book with ID {RentId}.";

            return;
        }

        _rent = restResponse.Data;
    }

    //private async Task ShowDialogRemoveRent()
    //{
    //    _errorMessage = string.Empty;

    //    var parameters = new DialogParameters
    //    {
    //        { nameof(DialogRemoveRent.RentId), RentId },
    //        { nameof(DialogRemoveRent.Title), _rent.Title }
    //    };

    //    var dialog = await DialogService.ShowAsync<DialogRemoveRent>("Remove Book", parameters);
    //    var dialogResult = await dialog.Result;

    //    if (dialogResult is null || dialogResult.Data is null)
    //    {
    //        _errorMessage = "Cancel remove Book.";

    //        return;
    //    }

    //    var isConfirmed = (bool)dialogResult.Data;

    //    if (isConfirmed)
    //    {
    //        Navigator.NavigateTo("Rents");
    //    }
    //}

    //private async Task ShowDialogAddReview()
    //{
    //    _errorMessage = string.Empty;

    //    var parameters = new DialogParameters
    //    {
    //        { nameof(DialogAddReview.RentId), RentId }
    //    };

    //    var dialog = await DialogService.ShowAsync<DialogAddReview>("Add Review", parameters);
    //    var dialogResult = await dialog.Result;

    //    if (dialogResult is null || dialogResult.Data is null)
    //    {
    //        _errorMessage = "Cancel add Review.";

    //        return;
    //    }

    //    var isConfirmed = (bool)dialogResult.Data;

    //    if (isConfirmed)
    //    {
    //        await LoadRent();
    //    }
    //}

    //private async Task ShowDialogRemoveReview(Guid reviewId)
    //{
    //    _errorMessage = string.Empty;

    //    var parameters = new DialogParameters
    //    {
    //        { nameof(DialogRemoveReview.ReviewId), reviewId }
    //    };

    //    var dialog = await DialogService.ShowAsync<DialogRemoveReview>("Remove Review", parameters);
    //    var dialogResult = await dialog.Result;

    //    if (dialogResult is null || dialogResult.Data is null)
    //    {
    //        _errorMessage = "Cancel remove Review.";

    //        return;
    //    }

    //    var isConfirmed = (bool)dialogResult.Data;

    //    if (isConfirmed)
    //    {
    //        await LoadRent();
    //    }
    //}
}
