using System;

namespace Combat.Common.Flags
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
