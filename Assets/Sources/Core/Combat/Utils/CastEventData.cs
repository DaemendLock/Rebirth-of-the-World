using Core.Combat.Abilities;
using Core.Combat.Units;
using static Core.Combat.Units.Components.Spellcasting;
using static UnityEngine.GraphicsBuffer;

namespace Core.Combat.Utils
{
    public readonly struct CastEventData
    {
        public readonly Unit Caster;
        public readonly Unit Target;
        public readonly Spell Spell;
        public readonly long TriggerTime;

        public CastEventData(Unit caster, Unit target, Spell spell)
        {
            Target = target;
            Caster = caster;
            Spell = spell;
            TriggerTime = CombatTime.Time;
        }

        public CastEventData(Unit caster, Unit target, Spell spell, long time)
        {
            Target = target;
            Caster = caster;
            Spell = spell;
            TriggerTime = time;
        }
    }

    
    public readonly struct HealthChangeEventData
    {
        public readonly float Value;
        public readonly Unit Caster;
        public readonly Unit Target;
        public readonly Spell Spell;

        public HealthChangeEventData(float value, Unit caster, Unit target, Spell spell)
        {
            Value = value;
            Target = target;
            Caster = caster;
            Spell = spell;
        }
    }

    /// <summary>
    /// Class <see cref="UnitEventData"/> represents resurection event BEFORE caster and target processing it.
    /// </summary>
    public readonly struct ResurrectionData
    {
        public readonly float Health;
        public readonly float Mana;

        public ResurrectionData(float health, float mana)
        {
            Health = health;
            Mana = mana;
        }
    }

    public class KillEventData
    {

    }
}
