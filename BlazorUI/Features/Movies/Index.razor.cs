using Cinema9.BlazorUI.Common.Statics;
using Cinema9.DataContracts.Movies.Queries.GetMovies;
using Microsoft.JSInterop;

namespace Cinema9.BlazorUI.Features.Movies;

public partial class Index
{
    private List<MovieIndex> _movies = [];

    bool _isLoading = false;
    private RestClient _restClient = default!;
    private string _errorMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        var baseUrl = Configuration["ApiBaseUrl"]!;
        _restClient = new RestClient(baseUrl);

        var restRequest = new RestRequest("Movies", Method.Get);
        var restResponse = await _restClient.ExecuteAsync<List<MovieIndex>>(restRequest);

        if (restResponse.Data is null)
        {
            _errorMessage = "Failed to retrieve Movies.";

            return;
        }

        _movies = restResponse.Data;

        _isLoading = false;
    }

    private async Task DownloadPdf()
    {
        _isLoading = true;
        _errorMessage = string.Empty;

        var restRequest = new RestRequest("Movies/GeneratePdf", Method.Get);
        var restResponse = await _restClient.DownloadDataAsync(restRequest);

        if (restResponse is null)
        {
            _errorMessage = "Failed to download Movies as a PDF file.";

            return;
        }

        var contentType = "application/pdf";
        var fileDownloadName = $"Movies_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.pdf";

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

        var restRequest = new RestRequest("Movies/GenerateExcel", Method.Get);
        var restResponse = await _restClient.DownloadDataAsync(restRequest);

        if (restResponse is null)
        {
            _errorMessage = "Failed to download Movies as an Excel file.";

            return;
        }

        var contentType = "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet";
        var fileDownloadName = $"Movies_{DateTime.Now:yyyy-MM-dd_HH-mm-ss}.xlsx";

        await JsRuntime.InvokeVoidAsync(
            JavaScriptIdentifierFor.DownloadFile,
            fileDownloadName,
            contentType,
            restResponse);

        _isLoading = false;
    }
}