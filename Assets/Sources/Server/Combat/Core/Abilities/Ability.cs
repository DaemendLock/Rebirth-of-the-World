using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils;
using Server.Combat.Core.Utils;
using Utils.DataTypes;

namespace Core.Combat.Abilities
{
    public class Ability : Castable
    {
        public Spell Spell { get; }
        public Duration Cooldown { get; private set; }

        public Ability(Spell spell)
        {
            Spell = spell;
        }

        public float CooldownTime => Cooldown.Left;

        public bool OnCooldown => Cooldown.Expired == false;

        public SchoolType Type => Spell.School;

        public CastActionRecord Cast(Unit caster, Unit target, SpellValueProvider values)
        {
            caster.SpendResource(values.Cost);
            StartCooldown(values.Cooldown, caster);
            CastActionRecord record = Spell.Cast(caster, target, values);

            return record;
        }

        public bool CanCast(Unit caster, Unit target, SpellValueProvider values) => Spell.CanCast(caster, target, values);

        public bool CanPay(CastInputData data, SpellModification spellModification)
        {
            return data.Caster != null && data.Caster.CanPay(Spell.Cost + spellModification.BonusCost);
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

        public void StartCooldown(float cooldown, Unit caster)
        {
            Cooldown = new Duration(StatsEvaluator.EvaluateAbilityCooldown(cooldown, caster, Spell.Flags.HasFlag(SpellFlags.HASTE_AFFECTS_COOLDOWN)));
        }

        public void ReduceCooldown(long time)
        {
            Cooldown -= time;
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

    public enum SpellSlot : byte
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
        public readonly SpellId SpellId;

        public InterruptData(bool succes, SpellId spellId, float duration)
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
