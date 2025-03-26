using Cinema9.DataContracts.Countries.Queries.GetCountriesLookup;
using Cinema9.DataContracts.Movies.Commands.CreateMovie;

namespace Cinema9.BlazorUI.Features.Movies;

public partial class Create
{
    private List<CountryLookup> _countries = default!;
    private CreateMovieModel _model = default!;
    private string _errorMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);

        var restRequest = new RestRequest("Countries/Lookup", Method.Get);
        var restResponse = await restClient.ExecuteAsync<List<CountryLookup>>(restRequest);

        if (restResponse.Data is not null)
        {
            _countries = restResponse.Data;
        }

        if (_countries.Count < 1)
        {
            _errorMessage = "There must be at least 1 Country.";

            return;
        }

        _model = new CreateMovieModel
        {
            Title = "",
            ReleaseDate = DateTime.Now,
            Budget = 0M,
            Country = _countries.First()
        };
    }

    private async Task Submit()
    {
        if (_model.ReleaseDate.HasValue == false)
        {
            Snackbar.Add("Silahkan pilih tanggal Release Date.", Severity.Error);

            return;
        }

        var input = new CreateMovieInput
        {
            Title = _model.Title,
            ReleaseDate = _model.ReleaseDate.Value,
            Budget = _model.Budget,
            CountryId = _model.Country.Id
        };

        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);

        var restRequest = new RestRequest("Movies", Method.Post);
        restRequest.AddBody(input);

        var restResponse = await restClient.ExecuteAsync<CreateMovieOutput>(restRequest);

        if (restResponse.Data is not null)
        {
            var output = restResponse.Data;

            Snackbar.Add($"Berhasil membuat Buku dengan judul {input.Title}.", Severity.Success);

            Navigator.NavigateTo($"/Movies/Details/{output.MovieId}");
        }
    }
}

public record CreateMovieModel
{
    public required string Title { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public required decimal Budget { get; set; }
    public required CountryLookup Country { get; set; }
}