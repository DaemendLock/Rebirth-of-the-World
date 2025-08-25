using System;

namespace Combat.Local.Domain.Flags
{
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
    }
}
