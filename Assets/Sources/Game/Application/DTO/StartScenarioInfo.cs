using Game.Domain.ValueObjects;

using Lobby.Common.Primitives;

using System;

namespace Game.Application.DTO
{
    public readonly struct ScenarioJoinInfo
    {
        public ScenarioJoinInfo(ScenarioId scenarioId, AccountId requestedBy)
        {
            ScenarioId = scenarioId;
            RequestedBy = requestedBy;
        }

        public ScenarioId ScenarioId { get; }
        public AccountId RequestedBy { get; }
    }

    public readonly struct StartScenarioInfo
    {
        public StartScenarioInfo(ScenarioId scenarioId, AccountId requestedBy)
        {
            if (scenarioId.Value == Guid.Empty)
            {
                throw new ArgumentException("A scenario is required.", nameof(scenarioId));
            }

            ScenarioId = scenarioId;
            RequestedBy = requestedBy;
        }

        public ScenarioId ScenarioId { get; }
        public AccountId RequestedBy { get; }
    }

    public readonly struct SelectCharacterInfo
    {
        public SelectCharacterInfo(AccountId requestedBy, CharacterSelection? characterSelection)
        {
            RequestedBy = requestedBy;
            CharacterSelection = characterSelection;
        }

        public AccountId RequestedBy { get; }
        public CharacterSelection? CharacterSelection { get; }
    }
}
