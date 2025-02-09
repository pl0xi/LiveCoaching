namespace LiveCoaching.Models.DTO;

public record GameDto(
    long GameId,
    string GameMode,
    string GameCreationDateComparedToCurrentTime,
    ExpanderHeaderColorGradient ExpanderHeaderColorGradient,
    string ChampionIconUrl,
    string ChampLevel
);