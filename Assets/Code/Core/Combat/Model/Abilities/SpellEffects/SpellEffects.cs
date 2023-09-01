using Core.Combat.Utils;
using Core.Stats;
using System.IO;
using Utils.Interfaces;

namespace Core.Combat.Abilities.SpellEffects
{
    public interface SpellEffect : SerializableInterface
    {
        public float GetValue(float modifyValue);

        public void ApplyEffect(EventData data, float modifyValue);
    }

    public interface ValueSource : SerializableInterface
    {
        float BaseValue { get; }

        float GetValue(EventData data, float modifyValue);
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

        public float GetValue(EventData data, float modifyValue) => data.Caster.EvaluateStat(UnitStat.SPELLPOWER).CalculatedValue * (BaseValue + modifyValue);

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

        public float GetValue(EventData data, float modifyValue) => data.Caster.EvaluateStat(UnitStat.ATK).CalculatedValue * (BaseValue + modifyValue);

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(BaseValue);
        }
    }
}