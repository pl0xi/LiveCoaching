using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Markup.Xaml;
using Avalonia.Media;
using LiveCoaching.Models.DTO;

namespace LiveCoaching.Controls;

public partial class MatchHistoryGame : UserControl
{
    
    public static readonly StyledProperty<ObservableCollection<GameDto>> MatchHistoryProperty =
        AvaloniaProperty.Register<MatchHistoryGame, ObservableCollection<GameDto>>(nameof(MatchHistory));
    
    public MatchHistoryGame()
    {
        InitializeComponent();

        if (Design.IsDesignMode)
        {
            MatchHistory = new ObservableCollection<GameDto>
            {
                new GameDto(
                    GameId: 1,
                    GameMode: "ARAM",
                    GameCreationDateComparedToCurrentTime: "2 days ago",
                    new ExpanderHeaderColorGradient(Color.Parse("#37D5D6"), Color.Parse("#35096D")),
                    ChampionIconUrl: "https://cdn.communitydragon.org/15.3.1/champion/113/square")
                ,
                
                new GameDto(
                    GameId: 1,
                    GameMode: "URF",
                    GameCreationDateComparedToCurrentTime: "3 days ago",
                    new ExpanderHeaderColorGradient(Color.Parse("#dd1818"), Color.Parse("#333333")),
                    ChampionIconUrl: "https://cdn.communitydragon.org/15.3.1/champion/13/square")
            };
        }
    }
    
    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
    
    public ObservableCollection<GameDto> MatchHistory
    {
        get => GetValue(MatchHistoryProperty);
        set => SetValue(MatchHistoryProperty, value);
    } 
}