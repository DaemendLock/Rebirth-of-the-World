using Combat.Common.Primitives;

using Lobby.Common.Primitives;

using System.Collections.Generic;

namespace Game.Domain.ValueObjects
{
    public readonly struct ScenarioMemberInfo
    {
        public ScenarioMemberInfo(
            AccountId accountId,
            CharacterKey? character,
            byte teamId,
            IReadOnlyCollection<int> skills = null)
        {
            AccountId = accountId;
            Character = character;
            TeamId = teamId;
            Skills = skills;
        }

        public AccountId AccountId { get; }
        public byte TeamId { get; }
        public CharacterKey? Character { get; }
        public IReadOnlyCollection<int> Skills { get; }
    }
}