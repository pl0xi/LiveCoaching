namespace LiveCoaching.Models.LCU;

public record Participant(
    int participantId,
    int championId,
    Stats? stats = null
); 