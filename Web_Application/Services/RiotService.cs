using Web_Application.DTO;
using Web_Application.Services.Interfaces;

namespace Web_Application.Services;

public class RiotService(HttpClient client) : IRiotService
{
   private readonly HttpClient _client = client;
   
   public async Task GetMatchHistory(SummonerDTO summoner)
   {
      var matchIds = await _client.GetFromJsonAsync<IEnumerable<string>>($"lol/match/v5/matches/by-puuid/{summoner.puuid}/ids");

      if (matchIds != null)
         foreach (var matchId in matchIds)
         {
            var match = await _client.GetFromJsonAsync<MatchDTO>($"lol/match/v5/matches/{matchId}");
         }
   }
}