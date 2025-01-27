using LiveCoaching.Services;
using ReactiveUI;
using System.Threading.Tasks;

namespace LiveCoaching.ViewModels;

public class HomeViewModel : ReactiveObject
{
    private string _leagueName = "Waiting on league client";

    public string LeagueName
    {
        get => _leagueName;
        set => this.RaiseAndSetIfChanged(ref _leagueName, value);
    }

    public async Task UpdateLeagueName()
    {
        if (LeagueUiClientManager.GetIsClientOpen())
        {
            var leagueName = await LeagueUiClientManager.GetLeagueName();
            LeagueName = leagueName ?? "Failed to get league display name";
        }
    }
}
