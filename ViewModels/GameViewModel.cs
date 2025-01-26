using ReactiveUI;

namespace LiveCoaching.ViewModels;
public class GameViewModel : ReactiveObject
{
    private readonly LeagueUiClientManager _leagueUiClientManager = new LeagueUiClientManager();
    public GameViewModel()
    {
    }

    public string title { get; } = "Waiting for champion select";
}