using Core.Combat.Abilities;
using Core.Combat.Units;

namespace Core.Combat.Utils
{
    public class EventData
    {
        public readonly Unit Caster;
        public readonly Unit Target;
        public readonly Spell Spell;
        public readonly long TriggerTime;

        public EventData(Unit caster, Unit target, Spell spell)
        {
            Target = target;
            Caster = caster;
            Spell = spell;
            TriggerTime = CombatTime.Time;
        }

        public EventData(Unit caster, Unit target, Spell spell, long time)
        {
            Target = target;
            Caster = caster;
            Spell = spell;
            TriggerTime = time;
        }
    }

    public class HealthChangeEventData : EventData
    {
        public readonly float Value;

        public HealthChangeEventData(float value, Unit caster, Unit target, Spell spell) : base(caster, target, spell)
        {
            Value = value;
        }
    }

    /// <summary>
    /// Class <see cref="UnitEventData"/> represents resurection event BEFORE caster and target processing it.
    /// </summary>
    public class ResurrectionData : EventData
    {
        public readonly float Health;
        public readonly float Mana;

        public ResurrectionData(Unit caster, Unit target, Spell spell, float health, float mana) : base(caster, target, spell)
        {
            Health = health;
            Mana = mana;
        }
    }

    public class KillEventData
    {

    }
}
