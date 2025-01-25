namespace LiveCoaching.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    public HomeViewModel HomeViewModel { get; } = new HomeViewModel();
    public GameViewModel GameViewModel { get; } = new GameViewModel();
}
