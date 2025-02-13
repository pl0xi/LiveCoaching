using System.Collections.Generic;

namespace LiveCoaching.Models.DataDragon;

public record SummonerResponse(
    Dictionary<string, SummonerSpell>? data
    );