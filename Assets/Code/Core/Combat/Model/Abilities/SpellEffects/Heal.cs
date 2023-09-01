using Core.Combat.Units;
using Core.Combat.Utils;
using Core.Stats;
using System.IO;
using Utils.Serializer;

namespace Core.Combat.Abilities.SpellEffects
{
    public class Heal : SpellEffect
    {
        private ValueSource _value;

        public Heal(ValueSource healing)
        {
            _value = healing;
        }

        public Heal(BinaryReader source)
        {
            _value = Serializer.Deserialize<ValueSource>(source);
        }

        public void ApplyEffect(EventData data, float modifyValue)
        {
            float originalHealing = _value.GetValue(data, modifyValue);

            Unit caster = data.Caster;

            if (caster != null)
            {
                originalHealing *= (1 + caster.EvaluateStat(UnitStat.HEALING_DONE).CalculatedValue) * caster.EvaluateVersalityMultiplyer();
            }

            HealthChangeEventData healing = new HealthChangeEventData(originalHealing, caster, data.Target, data.Spell);

            data.Target.Heal(healing);
        }

        public float GetValue(float modifyValue) => _value.BaseValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().ToString());
            Serializer.SerializeEffect(_value, buffer);
        }
    }
}