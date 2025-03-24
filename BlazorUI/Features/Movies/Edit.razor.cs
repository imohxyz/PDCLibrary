using Cinema9.DataContracts.Countries.Queries.GetCountriesLookup;
using Cinema9.DataContracts.Movies.Commands.UpdateMovie;
using Cinema9.DataContracts.Movies.Queries.GetMovie;
using Microsoft.AspNetCore.Components;

namespace Cinema9.BlazorUI.Features.Movies;

public partial class Edit
{
    [Parameter]
    public Guid MovieId { get; set; }

    private List<CountryLookup> _countries = default!;
    private EditMovieModel _model = default!;
    private string _errorMessage = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);

        var restRequestCountriesLookup = new RestRequest("Countries/Lookup", Method.Get);
        var restResponseCountriesLookup = await restClient.ExecuteAsync<List<CountryLookup>>(restRequestCountriesLookup);

        if (restResponseCountriesLookup.Data is not null)
        {
            _countries = restResponseCountriesLookup.Data;
        }

        if (_countries.Count < 1)
        {
            _errorMessage = "There must be at least 1 Country.";

            return;
        }

        var restRequestGetMovie = new RestRequest($"Movies/{MovieId}", Method.Get);
        var restResponseGetMovie = await restClient.ExecuteAsync<MovieDetail>(restRequestGetMovie);

        var movie = restResponseGetMovie.Data
            ?? throw new Exception("Movie not found.");

        _model = new EditMovieModel
        {
            Id = movie.Id,
            Title = movie.Title,
            Synopsis = movie.Synopsis,
            ReleaseDate = movie.ReleaseDate.LocalDateTime,
            Budget = movie.Budget,
            Country = _countries.Single(x => x.Id == movie.CountryId)
        };
    }

    private async Task Submit()
    {
        if (_model.ReleaseDate.HasValue == false)
        {
            Snackbar.Add("Silahkan pilih tanggal Release Date.", Severity.Error);

            return;
        }

        var input = new UpdateMovieInput
        {
            Id = _model.Id,
            Title = _model.Title,
            Synopsis = _model.Synopsis,
            ReleaseDate = _model.ReleaseDate.Value,
            Budget = _model.Budget,
            CountryId = _model.Country.Id
        };

        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);
        var restRequest = new RestRequest($"Movies/{MovieId}", Method.Put);
        restRequest.AddBody(input);

        var restResponse = await restClient.ExecuteAsync(restRequest);

        Snackbar.Add($"Berhasil update Movie dengan ID {_model.Id}.", Severity.Success);

        Navigator.NavigateTo($"/Movies/Details/{_model.Id}");
    }
}

public record EditMovieModel
{
    public required Guid Id { get; init; }
    public required string Title { get; set; }
    public required string? Synopsis { get; set; }
    public required DateTime? ReleaseDate { get; set; }
    public required decimal Budget { get; set; }
    public required CountryLookup Country { get; set; }
}
