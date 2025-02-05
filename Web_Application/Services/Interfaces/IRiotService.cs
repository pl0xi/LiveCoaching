using Web_Application.DTO;

namespace Web_Application.Services.Interfaces;

public interface IRiotService
{
    public Task GetMatchHistory(SummonerDTO summoner);
}