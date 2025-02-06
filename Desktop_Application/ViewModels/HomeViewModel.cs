using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LiveCoaching.Services.Api;
using ReactiveUI;

namespace LiveCoaching.ViewModels;

public class HomeViewModel : ReactiveObject
{
    private readonly LeagueClientApiService _leagueClientApiService;
    private bool _leagueDataLoaded;
    private string _leagueGameName = "Waiting on league client";
    private string _leagueSummonerLevel = "0";
    private string _leagueTagLine = "";
    private string _leagueUserIcon = "https://placehold.co/50.png";
    private Timer? _timer;

    public HomeViewModel(LeagueClientApiService leagueClientApiService)
    {
        _leagueClientApiService = leagueClientApiService;
        _timer = new Timer(async void (_) =>
            {
                try
                {
                    await UpdateLeagueSummonerAsync();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(5));
    }

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
        if (_leagueClientApiService.GetIsClientOpen())
        {
            var leagueSummoner = await _leagueClientApiService.GetLeagueSummonerAsync();

            if ((leagueSummoner == null) & (_leagueTagLine == "")) return;

            LeagueGameName = leagueSummoner?.gameName ?? "Failed to get league display name";
            LeagueTagLine = leagueSummoner?.tagLine != null ? $"#{leagueSummoner.tagLine}" : "";
            LeagueSummonerLevel = leagueSummoner?.summonerLevel != null ? $"LEVEL {leagueSummoner.summonerLevel}" : "0";
            LeagueUserIcon = leagueSummoner?.profileIconId != null
                ? $"https://ddragon.leagueoflegends.com/cdn/15.2.1/img/profileicon/{leagueSummoner.profileIconId}.png"
                : "https://placehold.co/80.png";
            LeagueDataLoaded = true;
        }
        else
        {
            LeagueGameName = "Waiting on league client";
            LeagueTagLine = "";
            LeagueDataLoaded = false;
        }
    }
}