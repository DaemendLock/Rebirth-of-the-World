using Core.Combat.Abilities;
using Core.Combat.Units;
using Core.Combat.Utils;
using System;

namespace Core.Combat.CastingBehaviors
{
    public abstract class CastingBehavior
    {
        private readonly CastEventData _data;
        private readonly SpellModification _modification;
        private readonly Duration _casttime;

        public CastingBehavior()
        {
            _casttime = new Duration(float.PositiveInfinity);
        }

        public CastingBehavior(CastEventData data, SpellModification modification)
        {
            float casttime = data.Spell.CastTime + modification.BonusCastTime.CalculatedValue;

            if (data.Caster != null)
            {
                casttime /= data.Caster.EvaluateHasteTimeDivider();
            }

            _data = data;
            _casttime = new Duration(casttime);
            _modification = modification;
        }

        public float TimeLeft => _casttime.Left;
        public float FullTime => _casttime.FullTime;
        public Unit Caster => _data.Caster;
        public Spell Spell => _data.Spell;

        public SchoolType School => _data.Spell.School;

        public virtual bool CanInterrupt => !(_data.Spell.Flags.HasFlag(SpellFlags.CANT_INTERRUPT) || _data.Caster.HasImmunity(Mechanic.Interrupt));

        public virtual bool Finished => _casttime.Expired;

        public virtual bool AllowAutoattack => false;

        public bool InterruptWithMovement => throw new NotImplementedException();

        public abstract void OnCastBegins();
        public abstract void OnUpdate();
        public abstract void OnCastEnd();

        protected void ProcSpell()
        {
            Castable ability = _data.Caster.FindAbility(_data.Spell);
            ability ??= _data.Spell;
            ability.Cast(_data, _modification);
        }

        protected void ProcSpell(Castable ability)
        {
            ability.Cast(_data, _modification);
        }
    }
}