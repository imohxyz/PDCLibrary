using Cinema9.BlazorUI.Common.Statics;
using Cinema9.DataContracts.Rents.Queries.GetRents;
using Microsoft.JSInterop;

namespace Cinema9.BlazorUI.Features.Rents;

public partial class Index
{
    private List<RentIndex> _rents = [];

    bool _isLoading = false;
    private RestClient _restClient = default!;
    private string _errorMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        var baseUrl = Configuration["ApiBaseUrl"]!;
        _restClient = new RestClient(baseUrl);

        var restRequest = new RestRequest("Rents", Method.Get);
        var restResponse = await _restClient.ExecuteAsync<List<RentIndex>>(restRequest);

        if (restResponse.Data is null)
        {
            _errorMessage = "Failed to retrieve Rents.";

            return;
        }

        _rents = restResponse.Data;

        _isLoading = false;
    }

    private async Task DownloadPdf()
    {
        _isLoading = true;
        _errorMessage = string.Empty;

        var restRequest = new RestRequest("Rents/GeneratePdf", Method.Get);
        var restResponse = await _restClient.DownloadDataAsync(restRequest);

        if (restResponse is null)
        {
            _errorMessage = "Failed to download Rents as a PDF file.";

            return;
        }

        var contentType = "application/pdf";
        var fileDownloadName = $"Rents_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf";

        await JsRuntime.InvokeVoidAsync(
            JavaScriptIdentifierFor.DownloadFile,
            fileDownloadName,
            contentType,
            restResponse);

        _isLoading = false;
    }

    private async Task DownloadExcel()
    {
        _isLoading = true;
        _errorMessage = string.Empty;

        var restRequest = new RestRequest("Rents/GenerateExcel", Method.Get);
        var restResponse = await _restClient.DownloadDataAsync(restRequest);

        if (restResponse is null)
        {
            _errorMessage = "Failed to download Rents as an Excel file.";

            return;
        }

        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        var fileDownloadName = $"Rents_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";

        await JsRuntime.InvokeVoidAsync(
            JavaScriptIdentifierFor.DownloadFile,
            fileDownloadName,
            contentType,
            restResponse);

        _isLoading = false;
    }
}