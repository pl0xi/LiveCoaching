using System;
using System.Threading;
using LiveCoaching.Services;
using ReactiveUI;

namespace LiveCoaching.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    private Timer? _timer;

    public MainWindowViewModel()
    {
        _timer = new Timer(_ => LeagueUiClientManager.SetClientStatus(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
    }

    public HomeViewModel HomeViewModel { get; } = new();
    public GameViewModel GameViewModel { get; } = new();
    public MatchHistoryViewModel MatchHistoryViewModel { get; } = new();
}