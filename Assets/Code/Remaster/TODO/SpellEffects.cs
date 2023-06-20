using Remaster.Events;
using Remaster.Interfaces;
using Remaster.Stats;
using Remaster.UnitComponents;
using System.IO;
using System.Numerics;

namespace Remaster.SpellEffects
{
    public interface ValueSource : SerializableInterface
    {
        float BaseValue { get; }

        float GetValue(EventData data, float modifyValue);
    }

    public class Interrupt : SpellEffect
    {
        public void ApplyEffect(EventData data, float modifyValue)
        {
            throw new System.NotImplementedException();
        }

        public float GetValue(float modifyValue)
        {
            throw new System.NotImplementedException();
        }
        public void Serialize(BinaryWriter buffer)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Resurrect : SpellEffect
    {
        public void ApplyEffect(EventData data, float modifyValue)
        {
            throw new System.NotImplementedException();
        }

        public float GetValue(float modifyValue)
        {
            throw new System.NotImplementedException();
        }
        public void Serialize(BinaryWriter buffer)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Summon : SpellEffect
    {
        public void ApplyEffect(EventData data, float modifyValue)
        {
            throw new System.NotImplementedException();
        }

        public float GetValue(float modifyValue)
        {
            throw new System.NotImplementedException();
        }
        public void Serialize(BinaryWriter buffer)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Taunt : SpellEffect
    {
        public void ApplyEffect(EventData data, float modifyValue)
        {
            throw new System.NotImplementedException();
        }

        public float GetValue(float modifyValue)
        {
            throw new System.NotImplementedException();
        }
        public void Serialize(BinaryWriter buffer)
        {
            throw new System.NotImplementedException();
        }
    }

    public class Teleport : SpellEffect
    {
        public void ApplyEffect(EventData data, float modifyValue)
        {
            throw new System.NotImplementedException();
        }

        public float GetValue(float modifyValue)
        {
            throw new System.NotImplementedException();
        }
        public void Serialize(BinaryWriter buffer)
        {
            throw new System.NotImplementedException();
        }
    }

    public class CreateProjectile : SpellEffect
    {
        private Spell _spell;
        private PositionComponent _position;
        private Vector3 _direction;
        private float _speed;

        public void ApplyEffect(EventData data, float modifyValue)
        {
            throw new System.NotImplementedException();
        }

        public float GetValue(float modifyValue)
        {
            throw new System.NotImplementedException();
        }
        public void Serialize(BinaryWriter buffer)
        {
            throw new System.NotImplementedException();
        }
    }

    public class FixedValue : ValueSource
    {
        public FixedValue(float value)
        {
            BaseValue = value;
        }

        public FixedValue(BinaryReader source)
        {
            BaseValue = source.ReadSingle();
        }

        public float BaseValue { get; }

        public float GetValue(EventData data, float modifyValue) => BaseValue + modifyValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(BaseValue);
        }
    }

    public class SpellpowerValue : ValueSource
    {
        public SpellpowerValue(float spellpowerMultiplier)
        {
            BaseValue = spellpowerMultiplier;
        }

        public SpellpowerValue(BinaryReader source)
        {
            BaseValue = source.ReadSingle();
        }

        public float BaseValue { get; }

        public float GetValue(EventData data, float modifyValue) => data.Caster.EvaluateDynamicStat(UnitStat.SPELLPOWER).CalculatedValue * (BaseValue + modifyValue);

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(BaseValue);
        }
    }

    public class AttackpowerValue : ValueSource
    {
        public AttackpowerValue(float spellpowerMultiplier)
        {
            BaseValue = spellpowerMultiplier;
        }

        public AttackpowerValue(BinaryReader source)
        {
            BaseValue = source.ReadSingle();
        }

        public float BaseValue { get; }

        public float GetValue(EventData data, float modifyValue) => data.Caster.GetStat(UnitStat.ATK) * (BaseValue + modifyValue);

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(BaseValue);
        }
    }
}