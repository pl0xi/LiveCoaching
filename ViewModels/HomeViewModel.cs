namespace LiveCoaching.ViewModels;

public class HomeViewModel : ViewModelBase
{
    private readonly LeagueUiClientManager _leagueUiClientManager = new LeagueUiClientManager();

    public string leagueName
    {
        get
        {
            if (_leagueUiClientManager.GetIsClientOpen())
            {
                return "League Username";
            }

            return "Waiting for league client";
        }
    }
}
