using System.Collections.Generic;

namespace LiveCoaching.Models.DataDragon;

public record SummonerData(
    Dictionary<string, SummonerSpell> data);