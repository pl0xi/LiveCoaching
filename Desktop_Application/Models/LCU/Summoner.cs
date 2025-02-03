namespace LiveCoaching.Models.LCU;

public record class Summoner(
    string? gameName = null,
    int? profileIconId = null,
    string? tagLine = null,
    int? summonerLevel = null
);