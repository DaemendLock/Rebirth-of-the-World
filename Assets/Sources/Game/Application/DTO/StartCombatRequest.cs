using Combat.Common.Primitives;

using System;
using System.Collections.Generic;

namespace Game.Application.DTO
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

    public struct CombatCharacterInfo
    {
        public CombatCharacterInfo(string character, byte teamId, bool controllable, IReadOnlyCollection<int> skills)
        {
            Character = new(character);
            TeamId = teamId;
            IsControllable = controllable;
            Skills = skills;
            Attributes = default;
            Resources = default;
        }

        public CharacterKey Character { get; set; }
        public byte TeamId { get; set; }
        public IReadOnlyCollection<int> Skills { get; set; }
        public bool IsControllable { get; set; }

        public CharacterAttributesInfo Attributes { get; set; }
        public CharacterResourceInfo Resources { get; set; }
    }

    public readonly struct CharacterAttributesInfo
    {
        public readonly float[] Values { get; }
    }

    public readonly struct CharacterResourceInfo
    {
        public readonly (ResourceId type, float maxValue, float initialValue)[] Values;
    }
}
