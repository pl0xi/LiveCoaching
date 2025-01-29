namespace LiveCoaching.Types;

public record class Summoner(
    string? gameName = null,
    int? profileIconId = null,
    string? tagLine = null,
    int? summonerLevel = null
);
