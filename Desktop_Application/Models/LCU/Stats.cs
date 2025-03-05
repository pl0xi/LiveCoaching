namespace LiveCoaching.Models.LCU;

public record Stats(
    bool win,
    int champLevel,
    int item0,
    int item1,
    int item2,
    int item3,
    int item4,
    int item5,
    int item6,
    int goldEarned,
    int kills,
    int deaths,
    int assists,
    int visionScore,
    int totalDamageTaken,
    int totalDamageDealt,
    int damageDealtToObjectives,
    int damageDealtToTurrets
);