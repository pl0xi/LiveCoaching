using System;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;
using LiveCoaching.Services;
using LiveCoaching.Types;
using ReactiveUI;

namespace LiveCoaching.ViewModels;

public class MatchHistoryViewModel : ReactiveObject
{
    private ObservableCollection<Game> _games = new();
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
            TimeSpan.FromSeconds(5));
    }

    public ObservableCollection<Game> MatchHistory
    {
        get => _games;
        set => this.RaiseAndSetIfChanged(ref _games, value);
    }

    private async Task UpdateLeagueMatchHistory()
    {
        if (LeagueUiClientManager.GetIsClientOpen())
        {
            var matchHistory = await LeagueUiClientManager.GetLeagueSummonerMatchHistoryAsync();

            if (matchHistory?.games?.games != null)
                MatchHistory = new ObservableCollection<Game>(matchHistory.games.games);
        }
    }
}