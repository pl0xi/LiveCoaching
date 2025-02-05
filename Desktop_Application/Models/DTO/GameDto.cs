namespace LiveCoaching.Models.DTO;

public record GameDto(
    long? GameId = null,
    string? GameMode = null,
    string? GameCreationDateComparedToCurrentTime = null
);