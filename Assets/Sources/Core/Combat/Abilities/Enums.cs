using System;

namespace Core.Combat.Abilities
{
    public enum DispellType
    {
        NONE = 0,
        MAGIC,
        BLEED
    }

    public enum GcdCategory : byte
    {
        NORMAL,
        IGNOR
    }

    public enum Mechanic
    {
        NONE = 0,
        INTERRUPT,
    }

    [Flags]
    public enum SchoolType : ushort
    {
        PHYSICAL = 1,
        THUNDER = 2,
        FIRE = 4,
        ICE = 8,
        GROUND = 16,
        WIND = 32,
        ORDER = 64,
        CHAOS = 128,
        DARKNESS = 256,
        LIGHT = 512,
        LIFE = 1024,
        DEATH = 2048,

        ANY = ushort.MaxValue,
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
        STATUS_STACK_COUNT_AFFECTS_BONUSES = 1024,
        TARGET_DEAD = 2048,
        DONT_DESTROY_ON_BREAK = 4096,
        SEPARATED_STATUS = 8192,
        PROC_SPELL = 16384,
        CAN_CAST_WHILE_CASTING = 32768,

    }

    [Flags]
    public enum TargetTeam : byte
    {
        NONE,
        ALLY,
        ENEMY,
        BOTH
    }
}
