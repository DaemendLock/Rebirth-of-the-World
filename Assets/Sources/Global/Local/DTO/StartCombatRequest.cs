using Combat.Common.ValueObjects;

using System;

namespace Global.Local.DTO
{
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

        public sealed class CombatCharacterInfo
        {
            public PlayerId Owner { get; }
            public int CharacterId { get; }
        }
    }
}
