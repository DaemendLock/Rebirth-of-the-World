using Core.Combat.Abilities.SpellEffects;
using Core.Combat.Utils;
using System;
using Utils;

namespace Core.Combat.Abilities
{
    public enum TargetTeam : byte
    {
        ALLY,
        ENEMY,
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

    }

    public enum DispellType
    {
        NONE = 0,
        MAGIC,
        BLEED,

    }

    public enum Mechanic
    {
        NONE = 0,
        INTERRUPT,

    }

    public interface Castable
    {
        void Cast(EventData data, SpellModification modification);
        CommandResult CanCast(EventData data, SpellModification modification);
    }

    public class Spell : Castable
    {
        private readonly SpellData _data;

        public Spell(SpellData data)
        {
            _data = data;
            Core.Data.SpellLib.SpellLib.RegisterSpell(this);
        }

        #if UNITY_EDITOR
        public SpellData Data => _data;
        #endif

        public string Icon => _data.Icon;

        public int Id => _data.Id;

        public AbilityCost Cost => _data.Cost;

        public TargetTeam TargetTeam => _data.TargetTeam;

        public float Range => _data.Range;

        public float Cooldown => _data.Cooldown;

        public float Duration => _data.Duration;

        public float CastTime => _data.CastTime;

        public float GCD => _data.GCD;

        public GcdCategory GcdCategory => _data.GcdCategory;

        public SchoolType School => _data.School;

        public SpellFlags Flags => _data.Flags;

        public int EffectsCount => _effects.Length;

        private SpellEffect[] _effects => _data.Effects;

        public virtual void Cast(EventData data, SpellModification modification)
        {
            for (int i = 0; i < EffectsCount; i++)
            {
                ApplyEffect(i, modification.EffectsModificationList[i], data);
            }

            data.Caster?.InformCast(data, CommandResult.SUCCES);
        }

        public virtual CommandResult CanCast(EventData data, SpellModification modification) => CommandResult.SUCCES;

        public bool IsDriving(Spell spell)
        {
            if (spell.Id == Id)
            {
                return true;
            }

            return false;
        }

        public float GetEffectValue(int index, float modifyValue) => _effects[index].GetValue(modifyValue);

        public void ApplyEffect(int index, float modifyValue, EventData data) => _effects[index].ApplyEffect(data, modifyValue);

        public sealed override int GetHashCode()
        {
            return Id;
        }

        public sealed override bool Equals(object obj)
        {
            return obj is Spell spell && spell.Id == Id;
        }
    }

    [Serializable]
    public readonly struct SpellData
    {
        public readonly int Id;
        public readonly string Icon;

        public readonly AbilityCost Cost;
        public readonly TargetTeam TargetTeam;
        public readonly float Range;
        public readonly float CastTime;
        public readonly float Cooldown;
        public readonly float GCD;
        public readonly GcdCategory GcdCategory;
        public readonly float Duration;
        public readonly SchoolType School;
        public readonly Mechanic Mechanic;
        public readonly DispellType DispellType;
        public readonly SpellFlags Flags;
        public readonly SpellEffect[] Effects;
        public readonly string Script;

        public SpellData(int id, string icon, AbilityCost cost, TargetTeam targetTeam, float range, float castTime, float cooldown, float gcd, GcdCategory gcdCategory, float duration, DispellType dispell, SchoolType school, Mechanic mechanic, SpellEffect[] effects, SpellFlags flags, Type script)
        {
            Id = id;
            Icon = icon;
            Cost = cost;
            TargetTeam = targetTeam;
            Range = range;
            CastTime = castTime;
            Cooldown = cooldown;
            GCD = gcd;
            GcdCategory = gcdCategory;
            Duration = duration;
            School = school;
            Mechanic = mechanic;
            DispellType = dispell;
            Effects = effects;
            Flags = flags;
            Script = script.ToString();
        }
    }

    //TODO: Remove
    [Serializable]
    public class TestSpellDataCreator
    {
        public int Id;
        public string Icon;

        public AbilityCost Cost;
        public TargetTeam TargetTeam;
        public float Range;
        public float CastTime;
        public float Cooldown;
        public float GCD;
        public GcdCategory GcdCategory;
        public float Duration;
        public SchoolType School;
        public Mechanic Mechanic;
        public DispellType DispellType;
        public SpellEffect[] Effects = new SpellEffect[0];
        public SpellFlags Flags;
        public string Script;

        public void Create()
        {
            Type type = Type.GetType(Script);

            if (type == null)
            {
                type = typeof(Spell);
            }

            SpellData data = new SpellData(Id, Icon, Cost, TargetTeam, Range, CastTime, Cooldown, GCD, GcdCategory, Duration, DispellType, School, Mechanic, Effects, Flags, type);

            Activator.CreateInstance(type, data);
        }
    }

    public class SpellModification
    {
        public AbilityCost BonusCost;
        public PercentModifiedValue BonusRange;
        public PercentModifiedValue BonusCastTime;
        public PercentModifiedValue CooldownReduction;
        public PercentModifiedValue BonusDuration;
        public readonly float[] EffectsModificationList;

        public SpellModification(Spell spell)
        {
            EffectsModificationList = new float[spell.EffectsCount];
        }
    }
}
