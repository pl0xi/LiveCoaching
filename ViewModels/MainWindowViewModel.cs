using LiveCoaching.Services;
using ReactiveUI;
using System;
using System.Threading;

namespace LiveCoaching.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private Timer? _timer;

    public MainWindowViewModel()
    {
        StartPeriodTimer();
    }


    public void StartPeriodTimer()
    {
        _timer = new Timer(_ => UpdateClientStatus(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    private void UpdateClientStatus()
    {
        LeagueUiClientManager.SetClientStatus();

        if (LeagueUiClientManager.GetIsClientOpen())
        {
            _ = HomeViewModel.UpdateLeagueSummonerAsync();
        }
    }

    public HomeViewModel HomeViewModel { get; } = new HomeViewModel();
    public GameViewModel GameViewModel { get; } = new GameViewModel();
}
