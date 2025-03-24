using Cinema9.DataContracts.Movies.Queries.GetMoviesLookup;

namespace Cinema9.BlazorUI.Features.Movies;

public partial class Lookup
{
    private List<MovieLookup> _movies = [];

    private MovieLookup? _selectedMovie = null;

    bool _isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);

        var restRequest = new RestRequest("Movies/Lookup", Method.Get);
        var restResponse = await restClient.ExecuteAsync<List<MovieLookup>>(restRequest);

        if (restResponse.Data is not null)
        {
            _movies = restResponse.Data;
        }

        _isLoading = false;
    }
}