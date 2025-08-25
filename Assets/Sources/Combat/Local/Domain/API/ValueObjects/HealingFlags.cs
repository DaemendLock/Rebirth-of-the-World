using System;

namespace Combat.Local.Domain.API.DTO
{
    [Flags]
    public enum HealingFlags
    {
        None = 0,
        CanRevive = 1,
        Reflected = 2,
        NonReactable = 4,
        ReviveHealing = NonReactable,
    }
}
