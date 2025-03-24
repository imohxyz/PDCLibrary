using Cinema9.DataContracts.Countries.Queries.GetCountries;

namespace Cinema9.BlazorUI.Features.Countries;

public partial class Index
{
    private List<CountryIndex> _countries = [];

    bool _isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);

        var restRequest = new RestRequest("Countries", Method.Get);
        var restResponse = await restClient.ExecuteAsync<List<CountryIndex>>(restRequest);

        if (restResponse.Data is not null)
        {
            _countries = restResponse.Data;
        }

        _isLoading = false;
    }
}
