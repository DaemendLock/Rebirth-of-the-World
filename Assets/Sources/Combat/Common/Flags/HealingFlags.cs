using System;

namespace Combat.Common.Flags
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
