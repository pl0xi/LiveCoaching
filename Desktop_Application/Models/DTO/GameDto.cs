using System.Collections.Generic;

namespace LiveCoaching.Models.DTO;

public record GameDto(
    long GameId,
    string GameMode,
    string GameCreationDateComparedToCurrentTime,
    ExpanderHeaderColorGradient ExpanderHeaderColorGradient,
    string ChampionIconUrl,
    string ChampLevel,
    List<ItemDto> Items,
    int GoldEarned,
    string spell1,
    string spell2
);