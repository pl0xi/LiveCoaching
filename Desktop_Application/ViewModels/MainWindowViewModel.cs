using ReactiveUI;

namespace LiveCoaching.ViewModels;

public class MainWindowViewModel : ReactiveObject
{
    public HomeViewModel HomeViewModel { get; } = Program.GetService<HomeViewModel>();
    public GameViewModel GameViewModel { get; } = new();
    public MatchHistoryViewModel MatchHistoryViewModel { get; } = Program.GetService<MatchHistoryViewModel>();
}