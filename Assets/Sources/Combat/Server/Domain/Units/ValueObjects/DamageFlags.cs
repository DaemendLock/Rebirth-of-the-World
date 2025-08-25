using System;

namespace Server.Combat.Domain.DTO
{
    [Flags]
    public enum DamageFlags
    {
        None = 0,
        NonLethal = 1,
        Reflected = 2,
        NonReactable = 4,
        IgnorTargetDef = 8,
    }
}
