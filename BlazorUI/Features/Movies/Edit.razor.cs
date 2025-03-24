using Cinema9.DataContracts.Movies.Commands.UpdateMovie;
using Cinema9.DataContracts.Movies.Queries.GetMovie;
using Microsoft.AspNetCore.Components;

namespace Cinema9.BlazorUI.Features.Movies;

public partial class Edit
{
    [Parameter]
    public Guid MovieId { get; set; }

    private EditMovieModel _model = default!;

    protected override async Task OnInitializedAsync()
    {
        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);
        var restRequest = new RestRequest($"Movies/{MovieId}", Method.Get);
        var restResponse = await restClient.ExecuteAsync<MovieDetail>(restRequest);

        var movie = restResponse.Data
            ?? throw new Exception("Movie not found.");

        _model = new EditMovieModel
        {
            Id = movie.Id,
            Title = movie.Title,
            Synopsis = movie.Synopsis,
            ReleaseDate = movie.ReleaseDate.LocalDateTime,
            Rating = movie.Rating,
            Budget = movie.Budget
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
            Rating = _model.Rating,
            Budget = _model.Budget
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
    public required float Rating { get; set; }
    public required decimal Budget { get; set; }
}
