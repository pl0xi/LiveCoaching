﻿namespace LiveCoaching.Models.DTO;

public record GameDto(
    long GameId,
    string? GameMode = null,
    string? GameCreationDateComparedToCurrentTime = null,
    ExpanderHeaderColorGradient? ExpanderHeaderColorGradient = null
);