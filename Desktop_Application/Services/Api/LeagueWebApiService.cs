using System.Linq;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;
using LiveCoaching.Models.DataDragon;

namespace LiveCoaching.Services.Api;

// This class utilizes two API's cdragon & ddragon.
public class LeagueWebApiService
{
    private readonly HttpClient _httpClient;
    private readonly SummonerData _summonerData;

    public LeagueWebApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _summonerData = FetchSummonerSpellsData().Result;
    }

    private async Task<SummonerData> FetchSummonerSpellsData()
    {
        var response =
            await _httpClient.GetAsync("https://ddragon.leagueoflegends.com/cdn/15.3.1/data/en_US/summoner.json");
        response.EnsureSuccessStatusCode();

        var jsonString = await response.Content.ReadAsStringAsync();
        var summonerResponse = JsonSerializer.Deserialize<SummonerResponse>(jsonString);

        return summonerResponse?.data;
    }

    public string GetSummonerSpellFileName(int spellId)
    {
        return _summonerData.data.Where(x => x.Value.key == spellId.ToString()).Select(x => x.Value.image.full)
            .FirstOrDefault();
    }
}