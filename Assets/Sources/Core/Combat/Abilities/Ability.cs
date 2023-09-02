using Core.Combat.Units;
using Core.Combat.Utils;
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
            //TODO Logger.Log($"Spell({Spell.Id}) casted: {data.Caster} -> {data.Target}");
        }

        public CommandResult CanCast(EventData data, SpellModification spellModification)
        {
            if (OnCooldown)
            {
                return CommandResult.ON_COOLDOWN;
            }

            return Spell.CanCast(data, spellModification);
        }

        public bool CanPay(EventData data, SpellModification spellModification)
        {
            return data.Caster != null && data.Caster.CanPay(Spell.Cost + spellModification.BonusCost) == false;
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
