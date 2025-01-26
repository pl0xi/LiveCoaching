using System;
using System.Threading;

namespace LiveCoaching.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly LeagueUiClientManager _leagueUiClientManager = new LeagueUiClientManager();
    private Timer _timer;

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
        _leagueUiClientManager.SetClientStatus();

        System.Diagnostics.Debug.WriteLine("Checking Client Status");

        if (_leagueUiClientManager.GetIsClientOpen())
        {
            _timer.Dispose();
        }
    }

    public HomeViewModel HomeViewModel { get; } = new HomeViewModel();
    public GameViewModel GameViewModel { get; } = new GameViewModel();
}
