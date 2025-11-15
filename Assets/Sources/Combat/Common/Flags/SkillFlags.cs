using System;

namespace Combat.Common.Flags
{

    [Flags]
    public enum ActionFlags
    {
        None = 0,
        AllowMovement = 1,
        CanInterrupt = 2,
        Holdable = 4,
    }

    [Flags]
    public enum SkillFlags : int
    {
        None = 0,
        Passive = 1,
        DontRestrictMovement = 2,
        CantInterrupt = 4,
        CantCrit = 8,
        HasteDontAffectsCooldown = 16,
        HasteDontAffectsRecovery = 32,
        ItemProvided = 64,
        WeaponAttack = 128,
        CantEvade = 256,
        CantParry = 512,
        CantBlock = 1024,
        CanTargetDead = 2048,
        ProcSpell = CantEvade | CantBlock | CantParry,
        Instant = 4096,
        StartCooldownOnImpact = 8192,
        IgnorCooldown = 16384,
        CanHold = 32768,
    }
}
