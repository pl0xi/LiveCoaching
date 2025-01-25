using System;
using System.Reactive.Linq;
using ReactiveUI;

namespace LiveCoaching.ViewModels;
public class GameViewModel : ReactiveObject
{
    private readonly LeagueUiClientManager _leagueUiClientManager = new LeagueUiClientManager();
    public GameViewModel()
    {
        Observable.Interval(TimeSpan.FromSeconds(2))
            .Subscribe(_ =>
            {
                bool isInChampionSelect = _leagueUiClientManager.isInChampionSelect();
            });
    }

    public string title { get; } = "Waiting for champion select";
}