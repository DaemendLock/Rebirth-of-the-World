using Core.Combat.Units;
using Core.Combat.Utils;
using Core.Stats;
using System.IO;
using Utils.Serializer;

namespace Core.Combat.Abilities.SpellEffects
{
    public class SchoolDamage : SpellEffect
    {
        private readonly ValueSource _value;

        public SchoolDamage(ValueSource damage)
        {
            _value = damage;
        }

        public SchoolDamage(BinaryReader source)
        {
            _value = Serializer.Deserialize<ValueSource>(source);
        }

        public void ApplyEffect(EventData data, float modifyValue)
        {
            float originalDamage = _value.GetValue(data, modifyValue);

            Unit caster = data.Caster;

            if (caster != null)
            {
                originalDamage *= (1 + caster.EvaluateStat(UnitStat.DAMAGE_DONE).CalculatedValue) * caster.EvaluateVersalityMultiplyer();
            }

            HealthChangeEventData damage = new HealthChangeEventData(originalDamage, data.Caster, data.Target, data.Spell);

            data.Target.Damage(damage);
        }

        public float GetValue(float modifyValue) => _value.BaseValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().ToString());
            Serializer.SerializeEffect(_value, buffer);
        }
    }
}