using Combat;
using System;

namespace Combat.SpellOld
{
    public abstract class OldSpellEffect
    {
        public OldSpellEffect(float value)
        {
            Value = value;
        }

        public float Value { get; }

        public abstract void ApplyEffect(Unit target, float modifyValue);
    }
    /*
    public sealed class SchoolDamage : SpellEffect
    {
        private DamageType _type;
        private bool _useSpellpower;

        public SchoolDamage(DamageType type, float value = 0, bool useSpellpower = true) : base(value)
        {
            _type = type;
            _useSpellpower = useSpellpower;
        }

        public override void ApplyEffect(Unit target, float modifyValue)
        {
            if (_useSpellpower)
            {
                RotW.ApplyDamage(new DamageEvent(target));
            }
            else
            {

            }
        }
    }

    public sealed class ApplyEffect : SpellEffect
    {
        public override void ApplyEffect(params object[] target)
        {
            throw new System.NotImplementedException();
        }
    }

    public sealed class SummonUnit
    {

    }

    public sealed class InterruptCast
    {

    }

    public sealed class GiveResource
    {

    }

    public sealed class OverrideSpell
    {

    }
    */
}

namespace Combat
{
    [Flags]
    public enum DamageType
    {
        Physical,
        Thunder,
        Fire,
        Ice,
        Wind,
        Ground,
        Order,
        Chaos,
        Darkness,
        Light,
        Life,
        Death
    }
}