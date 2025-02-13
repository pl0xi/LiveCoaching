using System;
using System.Collections.Generic;
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

    private readonly Lazy<Dictionary<string, SummonerSpell?>> _lazySummonerData;

    public LeagueWebApiService(HttpClient httpClient)
    {
        _httpClient = httpClient;
        _lazySummonerData = new Lazy<Dictionary<string, SummonerSpell>?>(() => FetchSummonerSpellsData().Result);
    }

    private async Task<Dictionary<string, SummonerSpell>> FetchSummonerSpellsData()
    {
        var response =
            await _httpClient.GetAsync("https://ddragon.leagueoflegends.com/cdn/15.3.1/data/en_US/summoner.json");

        var jsonString = await response.Content.ReadAsStringAsync();
        var summonerResponse = JsonSerializer.Deserialize<SummonerResponse>(jsonString);

        return summonerResponse?.data;
    }

    public string GetSummonerSpellFileName(int spellId)
    {
        var summonerData = _lazySummonerData.Value;
        return summonerData.FirstOrDefault(x => x.Value.key == spellId.ToString()).Value.image.full;
    }
}