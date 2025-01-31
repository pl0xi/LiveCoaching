using LiveCoaching.Services;
using LiveCoaching.Types;
using ReactiveUI;
using System.Threading.Tasks;

namespace LiveCoaching.ViewModels;

public class HomeViewModel : ReactiveObject
{
    private string _leagueGameName = "Waiting on league client";
    private string _leagueUserIcon = "https://placehold.co/50.png";
    private string _leagueTagLine = "";
    private string _leagueSummonerLevel = "0";
    private bool _leagueDataLoaded = false;

    public string LeagueGameName
    {
        get => _leagueGameName;
        set => this.RaiseAndSetIfChanged(ref _leagueGameName, value);
    }

    public string LeagueUserIcon
    {
        get => _leagueUserIcon;
        set => this.RaiseAndSetIfChanged(ref _leagueUserIcon, value);
    }

    public string LeagueTagLine
    {
        get => _leagueTagLine;
        set => this.RaiseAndSetIfChanged(ref _leagueTagLine, value);
    }

    public string LeagueSummonerLevel
    {
        get => _leagueSummonerLevel;
        set => this.RaiseAndSetIfChanged(ref _leagueSummonerLevel, value);
    }

    public bool LeagueDataLoaded
    {
        get => _leagueDataLoaded;
        set => this.RaiseAndSetIfChanged(ref _leagueDataLoaded, value);
    }


    public async Task UpdateLeagueSummonerAsync()
    {
        if (LeagueUiClientManager.GetIsClientOpen())
        {
            Summoner? leagueSummoner = await LeagueUiClientManager.GetLeagueSummoner();
            LeagueGameName = leagueSummoner?.gameName ?? "Failed to get league display name";
            LeagueTagLine = leagueSummoner?.tagLine != null ? $"#{leagueSummoner.tagLine}" : "";
            LeagueSummonerLevel = leagueSummoner?.summonerLevel != null ? $"LEVEL {leagueSummoner.summonerLevel}" : "0";
            LeagueUserIcon = leagueSummoner?.profileIconId != null ? $"https://ddragon.leagueoflegends.com/cdn/15.2.1/img/profileicon/{leagueSummoner.profileIconId}.png" : "https://placehold.co/80.png";
            LeagueDataLoaded = true;
        }
        else
        {
            LeagueDataLoaded = false;
        }
    }
}
