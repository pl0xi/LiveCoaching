using ReactiveUI;

namespace LiveCoaching.ViewModels;
public class GameViewModel : ReactiveObject
{
    public string title { get; } = "Waiting for champion select";
}