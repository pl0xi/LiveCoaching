using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LiveCoaching.Models.DTO;
using LiveCoaching.Services;
using ReactiveUI;

namespace LiveCoaching.ViewModels;

public class MatchHistoryViewModel : ReactiveObject
{
    private ObservableCollection<GameDto> _games = new();
    private Timer? _timer;

    public MatchHistoryViewModel()
    {
        _timer = new Timer(async void (_) =>
            {
                try
                {
                    await UpdateLeagueMatchHistory();
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e);
                }
            }, null, TimeSpan.Zero,
            TimeSpan.FromSeconds(30));
    }

    public ObservableCollection<GameDto> MatchHistory
    {
        get => _games;
        set => this.RaiseAndSetIfChanged(ref _games, value);
    }

    public async Task UpdateLeagueMatchHistory()
    {
        if (LeagueUiClientManager.GetIsClientOpen())
        {
            var games = await LeagueUiClientManager.GetLeagueSummonerMatchHistoryAsync();
            games?.ForEach(game =>
            {
                var index = MatchHistory.IndexOf(game);
                if (index == -1)
                    MatchHistory.Add(game);
            });
        }
    }
}