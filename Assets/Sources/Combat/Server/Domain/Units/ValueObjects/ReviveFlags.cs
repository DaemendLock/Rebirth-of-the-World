using System;

using Server.Combat.Domain.Units;

namespace Server.Combat.Domain.DTO
{
    [Flags]
    public enum ReviveFlags
    {
        None = 0,
        Healed = 1,
        Forced = 2,
        NonReactable = 4,
    }
}
