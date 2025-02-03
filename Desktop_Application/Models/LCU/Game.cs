using System.Collections.Generic;

namespace LiveCoaching.Models.LCU;

public record Game(
    long? gameId = null,
    string? gameCreationDate = null,
    string? gameMode = null,
    List<ParticipantIdentity>? participantIdentities = null
);