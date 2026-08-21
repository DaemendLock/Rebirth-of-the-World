using System;

namespace Combat.Common.Flags
{

    [Flags]
    public enum DamageFlags
    {
        None = 0,
        NonLethal = 1,
        Reflected = 2,
        NonReactable = 4,
        IgnorTargetDef = 8,
        InstantKill = 16,
        WeaponAttack = 32,
    }
}
