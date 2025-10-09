using System;

namespace Combat.Common.Flags
{
    [Flags]
    public enum KillFlags
    {
        None = 0,
        CantRevive = 1,
        Forced = 2,
        NonReactable = 4,
    }
}
