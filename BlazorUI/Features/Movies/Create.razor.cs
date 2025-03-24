using Cinema9.DataContracts.Movies.Commands.CreateMovie;

namespace Cinema9.BlazorUI.Features.Movies;

public partial class Create
{
    private CreateMovieModel _model = default!;

    protected override void OnInitialized()
    {
        _model = new CreateMovieModel
        {
            Title = "The Matrix",
            ReleaseDate = DateTime.Now.AddYears(-10),
            Budget = 50_000_000M
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
            Budget = _model.Budget
        };

        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);

        var restRequest = new RestRequest("Movies", Method.Post);
        restRequest.AddBody(input);

        var restResponse = await restClient.ExecuteAsync<CreateMovieOutput>(restRequest);

        if (restResponse.Data is not null)
        {
            var output = restResponse.Data;

            Snackbar.Add($"Berhasil membuat Movie dengan judul {input.Title}.", Severity.Success);

            Navigator.NavigateTo($"/Movies/Details/{output.MovieId}");
        }
    }
}

public record CreateMovieModel
{
    public required string Title { get; set; }
    public DateTime? ReleaseDate { get; set; }
    public required decimal Budget { get; set; }
}