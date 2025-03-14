﻿using System.Collections.Generic;

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
    string spell2,
    int Kills,
    int Deaths,
    int Assists,
    double Kda,
    int VisionScore,
    int DamageTaken,
    int DamageDealt,
    int DamageDealtToObjectives,
    int DamageDealtToTurrets,
    int TrueDamageDealt,
    int DamageSelfMitigated,
    int MagicDamageDealt
);