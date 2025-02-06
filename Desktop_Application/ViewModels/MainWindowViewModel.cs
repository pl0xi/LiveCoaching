using System;
using System.Diagnostics;
using System.Threading;
using LiveCoaching.Services;
using ReactiveUI;

namespace LiveCoaching.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private bool _firstTimeClientOpen;
    private Timer? _timer;

    public MainWindowViewModel()
    {
        _timer = new Timer(async void (_) =>
        {
            try
            {
                LeagueUiClientManager.SetClientStatus();
                if (!_firstTimeClientOpen && LeagueUiClientManager.GetIsClientOpen())
                {
                    await HomeViewModel.UpdateLeagueSummonerAsync();
                    await MatchHistoryViewModel.UpdateLeagueMatchHistory();
                    _firstTimeClientOpen = true;
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e);
            }
        }, null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    public HomeViewModel HomeViewModel { get; } = new();
    public GameViewModel GameViewModel { get; } = new();
    public MatchHistoryViewModel MatchHistoryViewModel { get; } = new();
}