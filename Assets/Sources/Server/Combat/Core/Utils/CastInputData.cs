using Core.Combat.Abilities;
using Core.Combat.Units;
using Utils.DataTypes;

namespace Core.Combat.Utils
{
    public readonly struct CastInputData
    {
        public readonly Unit Caster;
        public readonly Unit Target;
        public readonly SpellSlot SpellSlot;

        public CastInputData(Unit caster, Unit target, SpellSlot slot)
        {
            Target = target;
            Caster = caster;
            SpellSlot = slot;
        }
    }

    public readonly struct HealthChangeEventData
    {
        public readonly float Value;
        public readonly Unit Caster;
        public readonly Unit Target;
        public readonly SpellId Spell;

        public HealthChangeEventData(float value, Unit caster, Unit target, SpellId source)
        {
            Value = value;
            Target = target;
            Caster = caster;
            Spell = source;
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
