using System.Collections.Generic;
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
            MatchHistory = new ObservableCollection<GameDto>
            {
                new(
                    1,
                    "ARAM",
                    "2 days ago",
                    new ExpanderHeaderColorGradient(Color.Parse("#37D5D6"), Color.Parse("#35096D")),
                    "https://cdn.communitydragon.org/15.3.1/champion/113/square",
                    "Level 18",
                    new List<ItemDto>
                    {
                        new("https://ddragon.leagueoflegends.com/cdn/15.3.1/img/item/1055.png"),
                        new("https://ddragon.leagueoflegends.com/cdn/15.3.1/img/item/3142.png"),
                        new("https://ddragon.leagueoflegends.com/cdn/15.3.1/img/item/3742.png"),
                        new("https://ddragon.leagueoflegends.com/cdn/15.3.1/img/item/3748.png"),
                        new("https://ddragon.leagueoflegends.com/cdn/15.3.1/img/item/3814.png"),
                        new("https://ddragon.leagueoflegends.com/cdn/15.3.1/img/item/4401.png"),
                        new(
                            "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/assets/items/icons2d/gp_ui_placeholder.png")
                    },
                    8999, "https://ddragon.leagueoflegends.com/cdn/15.3.1/img/spell/SummonerFlash.png",
                    "https://ddragon.leagueoflegends.com/cdn/15.3.1/img/spell/SummonerTeleport.png"
                ),

                new(
                    1,
                    "URF",
                    "3 days ago",
                    new ExpanderHeaderColorGradient(Color.Parse("#dd1818"), Color.Parse("#333333")),
                    "https://cdn.communitydragon.org/15.3.1/champion/13/square",
                    "Level 12",
                    new List<ItemDto>
                    {
                        new("https://ddragon.leagueoflegends.com/cdn/15.3.1/img/item/1055.png"),
                        new("https://ddragon.leagueoflegends.com/cdn/15.3.1/img/item/3142.png"),
                        new(
                            "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/assets/items/icons2d/gp_ui_placeholder.png"),
                        new(
                            "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/assets/items/icons2d/gp_ui_placeholder.png"),
                        new(
                            "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/assets/items/icons2d/gp_ui_placeholder.png"),

                        new(
                            "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/assets/items/icons2d/gp_ui_placeholder.png"),
                        new(
                            "https://raw.communitydragon.org/latest/plugins/rcp-be-lol-game-data/global/default/assets/items/icons2d/gp_ui_placeholder.png"
                        )
                    },
                    12034, "https://ddragon.leagueoflegends.com/cdn/15.3.1/img/spell/SummonerFlash.png",
                    "https://ddragon.leagueoflegends.com/cdn/15.3.1/img/spell/SummonerTeleport.png"
                )
            };
    }

    public ObservableCollection<GameDto> MatchHistory
    {
        get => GetValue(MatchHistoryProperty);
        set => SetValue(MatchHistoryProperty, value);
    }

    private void InitializeComponent()
    {
        AvaloniaXamlLoader.Load(this);
    }
}