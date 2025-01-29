using LiveCoaching.Services;
using LiveCoaching.Types;
using ReactiveUI;
using System.Threading.Tasks;

namespace LiveCoaching.ViewModels;

public class HomeViewModel : ReactiveObject
{
    private string _leagueGameName = "Waiting on league client";
    private string _leagueUserIcon = "https://placehold.co/50";
    private string _leagueTagLine = "";
    private string _leagueSummonerLevel = "0";

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

    public string LeaugeSummonerLevel
    {
        get => _leagueSummonerLevel;
        set => this.RaiseAndSetIfChanged(ref _leagueSummonerLevel, value);
    }


    public async Task UpdateLeagueSummoner()
    {
        if (LeagueUiClientManager.GetIsClientOpen())
        {
            Summoner? leagueSummoner = await LeagueUiClientManager.GetLeagueSummoner();
            LeagueGameName = leagueSummoner?.gameName ?? "Failed to get league display name";
            LeagueTagLine = leagueSummoner?.tagLine != null ? "#" + leagueSummoner?.tagLine : "";
            LeaugeSummonerLevel = leagueSummoner?.summonerLevel != null ? "LEVEL " + leagueSummoner?.summonerLevel.ToString() : "0";
            LeagueUserIcon = leagueSummoner?.profileIconId != null ? "https://ddragon.leagueoflegends.com/cdn/15.2.1/img/profileicon/" + leagueSummoner?.profileIconId + ".png" : "https://placehold.co/50";
        }
    }
}
