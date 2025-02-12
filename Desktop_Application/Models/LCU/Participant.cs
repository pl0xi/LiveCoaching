namespace LiveCoaching.Models.LCU;

public record Participant(
    int participantId,
    int championId,
    Stats stats,
    int spell1Id,
    int spell2Id
);