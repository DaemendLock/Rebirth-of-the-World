using Combat.Common.Primitives;

using Lobby.Common.Primitives;

using System;
using System.Collections.Generic;

namespace Game.Application.DTO
{
    public readonly struct CreateCombatRequest
    {
        public CreateCombatRequest(
            ScenarioId scenarioId,
            AccountId requestedBy,
            string locationName,
            IReadOnlyList<CombatParticipant> participants)
        {
            ScenarioId = scenarioId;
            RequestedBy = requestedBy;
            LocationName = locationName;
            Participants = participants ?? throw new ArgumentNullException(nameof(participants));
        }

        public ScenarioId ScenarioId { get; }
        public AccountId RequestedBy { get; }
        public string LocationName { get; }
        public IReadOnlyList<CombatParticipant> Participants { get; }
    }

    public readonly struct CombatParticipant
    {
        public CombatParticipant(
            AccountId ownerId,
            CharacterKey character,
            byte teamId,
            IReadOnlyCollection<int> skills)
        {
            OwnerId = ownerId;
            Character = character;
            TeamId = teamId;
            Skills = skills;
        }

        public AccountId OwnerId { get; }
        public CharacterKey Character { get; }
        public byte TeamId { get; }
        public IReadOnlyCollection<int> Skills { get; }
    }
}
