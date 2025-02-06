using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using LiveCoaching.Models.DTO;
using LiveCoaching.Services.Api;
using ReactiveUI;

namespace LiveCoaching.ViewModels;

public class MatchHistoryViewModel : ReactiveObject
{
    private readonly LeagueClientApiService _leagueClientApiService;
    private ObservableCollection<GameDto> _games = new();

    public MatchHistoryViewModel(LeagueClientApiService leagueClientApiService)
    {
        _leagueClientApiService = leagueClientApiService;
        _leagueClientApiService.ClientStatusChanged += UpdateLeagueMatchHistory;
    }

    public ObservableCollection<GameDto> MatchHistory
    {
        get => _games;
        set => this.RaiseAndSetIfChanged(ref _games, value);
    }

    public async void UpdateLeagueMatchHistory()
    {
        try
        {
            if (_leagueClientApiService.GetIsClientOpen())
            {
                var games = await _leagueClientApiService.GetLeagueSummonerMatchHistoryAsync();
                games?.ForEach(game =>
                {
                    var index = MatchHistory.IndexOf(game);
                    if (index == -1)
                        MatchHistory.Add(game);
                });
            }
        }
        catch (Exception e)
        {
            Debug.WriteLine(e);
        }
    }
}