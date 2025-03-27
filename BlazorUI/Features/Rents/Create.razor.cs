using Cinema9.DataContracts.Movies.Queries.GetMoviesLookup;
using Cinema9.DataContracts.Rents.Commands.CreateRent;

namespace Cinema9.BlazorUI.Features.Rents;

public partial class Create
{
    private List<MovieLookup> _movies = default!;
    private CreateRentModel _model = default!;
    private string _errorMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);

        var restRequest = new RestRequest("Movies/Lookup", Method.Get);
        var restResponse = await restClient.ExecuteAsync<List<MovieLookup>>(restRequest);

        if (restResponse.Data is not null)
        {
            _movies = restResponse.Data;
        }

        if (_movies.Count < 1)
        {
            _errorMessage = "There must be at least 1 Books.";

            return;
        }

        _model = new CreateRentModel
        {
            Qty = 0,
            Movie = _movies.First()
        };
    }

    private async Task Submit()
    {
        var input = new CreateRentInput
        {
            Qty = _model.Qty,
            MovieId = _model.Movie.Id
        };

        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);

        var restRequest = new RestRequest("Rents", Method.Post);
        restRequest.AddBody(input);

        var restResponse = await restClient.ExecuteAsync<CreateRentOutput>(restRequest);

        if (restResponse.Data is not null)
        {
            var output = restResponse.Data;

            Snackbar.Add($"Berhasil simpan data transaksi.", Severity.Success);

            Navigator.NavigateTo($"/Rents/Details/{output.RentId}");
        }
    }
}

public record CreateRentModel
{
    public required int Qty { get; set; }
    public required MovieLookup Movie { get; set; }
}