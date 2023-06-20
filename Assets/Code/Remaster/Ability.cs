using Remaster.Events;
using Remaster.Utils;
using System;
using UnityEngine;

namespace Remaster
{
    public interface IReadOnlyAbility
    {
        public Spell Spell { get; }
        public Duration Cooldown { get; }
        public Sprite Icon { get; }
        public float CooldownTime { get; }
        public bool OnCooldown { get; }
        public SchoolType Type { get; }
    }

    public class Ability : IReadOnlyAbility, Castable
    {
        public Spell Spell { get; }
        public Duration Cooldown { get; private set; }

        public Ability(Spell spell)
        {
            Spell = spell;
        }

        public Sprite Icon => throw new NotImplementedException();

        public float CooldownTime => Cooldown.Left;

        public bool OnCooldown => Cooldown.Expired == false;

        public SchoolType Type => Spell.School;

        public void Cast(EventData data, SpellModification modification)
        {
            CommandResult result = CanCast(data, modification);

            if (result != CommandResult.SUCCES)
            {
                return;
            }

            data.Caster?.SpendResource(Spell.Cost);

            StartCooldown(modification.CooldownReduction, data.Caster);
            Spell.Cast(data, modification);
            Logger.Log($"Spell({Spell.Id}) casted: {data.Caster} -> {data.Target}");
        }

        public CommandResult CanCast(EventData data, SpellModification spellModification)
        {
            if (OnCooldown)
            {
                return CommandResult.ON_COOLDOWN;
            }

            if (data.Caster != null && data.Caster.CanPay(Spell.Cost + spellModification.BonusCost) == false)
            {
                return CommandResult.NOT_ENOUGHT_RESOURCE;
            }

            if (Spell.CanCast(data) == false)
            {
                return CommandResult.INVALID_TARGET;
            }

            return CommandResult.SUCCES;
        }

        #region Cooldown
        public void ResetCooldown()
        {
            Cooldown = new Duration();
        }

        public void SetCooldown(float time)
        {
            Cooldown = new Duration(time);
        }

        public void StartCooldown(PercentModifiedValue modification, Unit caster)
        {
            PercentModifiedValue cooldown = new PercentModifiedValue(Spell.Cooldown, 100) + modification;

            if (caster == null || (Spell.Flags & SpellFlags.HASTE_AFFECTS_COOLDOWN) == 0)
            {
                Cooldown = new Duration(cooldown.CalculatedValue);
            }
            else
            {
                Cooldown = new Duration(cooldown.CalculatedValue / caster.EvaluateHasteTimeDivider());
            }
        }

        public void ReduceCooldown(float time)
        {
            Cooldown += -time;
        }
        #endregion

        public bool IsDriving(Spell spell)
        {
            return Spell.IsDriving(spell);
        }

        public bool SpellEquals(Spell spell)
        {
            return Spell.Id == spell.Id;
        }
    }

    [Serializable]
    public readonly struct AbilityCost
    {
        public readonly static AbilityCost None = new AbilityCost(0f, 0f);

        public readonly float Left;
        public readonly float Right;

        public AbilityCost(float left, float right)
        {
            Left = left;
            Right = right;
        }

        public static AbilityCost operator +(AbilityCost value1, AbilityCost value2) => new AbilityCost(value1.Left + value2.Left, value1.Right + value2.Right);
    }

    public enum AbilityBehavior
    {
        NONE = 0,
        NO_TARGET,
        UNIT_TARGET,
        DIRECTION,
        PASSIVE
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

    public enum GcdCategory : byte
    {
        NORMAL,
        IGNOR
    }

    public enum SpellSlot
    {
        FIRST,
        SECOND,
        THIRD,
        FOURTH,
        FIVETH,
        SIXTH,
        ITEM_1,
        ITEM_2,
        ATTACK_MAIN,
        ATTACK_OFF_HAND,
    }

    public readonly struct InterruptData
    {
        public readonly bool Succes;
        public readonly float Duration;
        public readonly int SpellId;

        public InterruptData(bool succes, int spellId, float duration)
        {
            Succes = succes;
            Duration = duration;
            SpellId = spellId;
        }

        public InterruptData(bool succes, Spell spell, float duration)
        {
            Succes = succes;
            Duration = duration;
            SpellId = spell.Id;
        }
    }
}
