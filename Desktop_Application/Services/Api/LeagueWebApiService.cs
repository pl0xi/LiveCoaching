using System.Net.Http;

namespace LiveCoaching.Services.Api;

// This class utilizes two API's cdragon & ddragon.
public class LeagueWebApiService
{
    private readonly HttpClient _httpClient;

    public LeagueWebApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
}