using Combat.Common.Primitives;

using System;
using System.Collections.Generic;

namespace Global.Local.DTO
{
    public readonly struct CharacterId
    {
        public readonly int Value;
    }

    public sealed class StartCombatRequest
    {
        public StartCombatRequest(string locationName)
        {
            if (string.IsNullOrWhiteSpace(locationName))
            {
                throw new ArgumentException("A combat location scene is required.", nameof(locationName));
            }

            LocationName = locationName;
        }

        public string LocationName { get; }
        public IReadOnlyCollection<CombatCharacterInfo> Characters { get; }
    }

    public readonly struct CombatCharacterInfo
    {
        public PlayerId Owner { get; }
        public CharacterId Id { get; }
        public byte TeamId { get; }
    }
}
