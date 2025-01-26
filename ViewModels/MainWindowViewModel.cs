using System;
using System.Reactive.Linq;

namespace LiveCoaching.ViewModels;

public class MainWindowViewModel : ViewModelBase
{
    private readonly LeagueUiClientManager _leagueUiClientManager = new LeagueUiClientManager();

    public MainWindowViewModel()
    {
        Observable.Generate(
         initialState: _leagueUiClientManager.getIsClientOpen(),
         condition: _ => true,
         iterate: _ =>
         {
             _leagueUiClientManager.setClientStatus();
             return _leagueUiClientManager.getIsClientOpen();
         },
         resultSelector: state => state,
         timeSelector: state => state ? TimeSpan.FromSeconds(10) : TimeSpan.FromSeconds(2)
            )
         .Subscribe();
    }

    public HomeViewModel HomeViewModel { get; } = new HomeViewModel();
    public GameViewModel GameViewModel { get; } = new GameViewModel();
}
