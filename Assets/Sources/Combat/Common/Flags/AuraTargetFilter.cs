using System;

namespace Combat.Common.Flags
{
    [Flags]
    public enum AuraTargetFilter
    {
        None = 0,
        Self = 1,
        Allies = 2,
        Enemies = 4,
        Dead = 8
    }
}
