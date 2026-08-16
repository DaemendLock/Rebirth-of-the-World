using Combat.Common.Primitives;

using System;
using System.Collections.Generic;
using System.Linq;

namespace Global.Local.DTO
{
    public sealed class StartCombatRequest
    {
        public StartCombatRequest(string locationName, IReadOnlyCollection<CombatCharacterInfo> characters)
        {
            if (string.IsNullOrWhiteSpace(locationName))
            {
                throw new ArgumentException("A combat location scene is required.", nameof(locationName));
            }

            LocationName = locationName;
            Characters = characters;
        }

        public string LocationName { get; }
        public IReadOnlyCollection<CombatCharacterInfo> Characters { get; }
    }

    public readonly struct CombatCharacterInfo
    {
        public CombatCharacterInfo(string character, byte teamId)
        {
            Character = new(character);
            TeamId = teamId;
        }

        public CharacterKey Character { get; }
        public byte TeamId { get; }
    }
}
