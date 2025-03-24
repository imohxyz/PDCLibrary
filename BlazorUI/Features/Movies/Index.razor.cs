using Cinema9.DataContracts.Movies.Queries.GetMovies;

namespace Cinema9.BlazorUI.Features.Movies;

public partial class Index
{
    private List<MovieIndex> _movies = [];

    bool _isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);

        var restRequest = new RestRequest("Movies", Method.Get);
        var restResponse = await restClient.ExecuteAsync<List<MovieIndex>>(restRequest);

        if (restResponse.Data is not null)
        {
            _movies = restResponse.Data;
        }

        _isLoading = false;
    }
}