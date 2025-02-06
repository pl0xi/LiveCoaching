using System.Collections.Generic;

namespace LiveCoaching.Models.LCU;

public record Game(
    long gameId,
    string gameCreationDate,
    string gameMode,
    List<ParticipantIdentity> participantIdentities,
    List<Participant> participants
);