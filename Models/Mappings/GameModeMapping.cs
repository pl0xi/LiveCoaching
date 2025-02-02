using System.Collections.Generic;

namespace LiveCoaching.Types.Mappings;

public class GameModeMapping
{
    // https://static.developer.riotgames.com/docs/lol/gameModes.json
    public static readonly Dictionary<string?, string> Modes = new()
    {
        { "CLASSIC", "Summoners Rift 5v5" },
        { "ODIN", "Dominion/Crystal Scar" },
        { "ARAM", "ARAM" },
        { "TUTORIAL", "Tutorial" },
        { "URF", "URF" },
        { "DOOMBOTSTEEMO", "Doom Bots" },
        { "ONEFORALL", "One for All" },
        { "ASCENSION", "Ascension" },
        { "FIRSTBLOOD", "Snowdown Showdown" },
        { "KINGPORO", "Legend of the Poro King" },
        { "SIEGE", "Nexus Siege" },
        { "ASSASSINATE", "Blood Hunt Assassin" },
        { "ARSR", "All Random Summoners Rift" },
        { "DARKSTAR", "Dark Star: Singularity" },
        { "STARGUARDIAN", "Star Guardian Invasion" },
        { "PROJECT", "PROJECT: Hunters" },
        { "GAMEMODEX", "Nexus Blitz" },
        { "ODYSSEY", "Odyssey: Extraction" },
        { "NEXUSBLITZ", "Nexus Blitz" },
        { "ULTBOOK", "Ultimate Spellbook" }
    };
}