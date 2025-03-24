using Cinema9.DataContracts.Countries.Queries.GetCountriesLookup;

namespace Cinema9.BlazorUI.Features.Countries;

public partial class Lookup
{
    private List<CountryLookup> _countries = [];
    private CountryLookup? _selectedCountry = null;

    bool _isLoading = false;

    protected override async Task OnInitializedAsync()
    {
        _isLoading = true;

        var baseUrl = Configuration["ApiBaseUrl"]!;
        var restClient = new RestClient(baseUrl);

        var restRequest = new RestRequest("Countries/Lookup", Method.Get);
        var restResponse = await restClient.ExecuteAsync<List<CountryLookup>>(restRequest);

        if (restResponse.Data is not null)
        {
            _countries = restResponse.Data;
        }

        _isLoading = false;
    }
}