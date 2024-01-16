using System;

namespace Core.Combat.Abilities
{
    public enum GcdCategory : byte
    {
        Normal,
        Ignor
    }

    public enum Mechanic
    {
        None = 0,
        Interrupt,
        Attack,
    }

    [Flags]
    public enum SchoolType : ushort
    {
        Physical = 1,
        Thunder = 2,
        Fire = 4,
        Ice = 8,
        Ground = 16,
        Wind = 32,
        Order = 64,
        Chaos = 128,
        Darkness = 256,
        Light = 512,
        Life = 1024,
        Death = 2048,

        Any = ushort.MaxValue,
    }

    [Flags]
    public enum SpellFlags : int
    {
        NONE = 0,
        PASSIVE_SPELL = 1,
        CAN_CAST_WHILE_MOVING = 2,
        CANT_INTERRUPT = 4,
        CANT_CRIT = 8,
        HASTE_AFFECTS_COOLDOWN = 16,
        ITEM_PROVIDED_SPELL = 32,
        AUTOATTACK = 64,
        CANT_BE_EVAIDED = 128,
        CANT_BE_PARRIED = 256,
        CANT_BE_BLOCKED = 512,
        TARGET_DEAD = 1024,
        PROC_SPELL = 2048,
        CAN_CAST_WHILE_CASTING = 4096,
    }

    [Flags]
    public enum TargetTeam : byte
    {
        None,
        Ally,
        Enemy,
        Both
    }
}
