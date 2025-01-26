namespace LiveCoaching.ViewModels;

public class HomeViewModel : ViewModelBase
{
    private readonly LeagueUiClientManager _leagueUiClientManager = new LeagueUiClientManager();

    public string leagueName
    {
        get { return "Live Coaching"; }
    }
}
