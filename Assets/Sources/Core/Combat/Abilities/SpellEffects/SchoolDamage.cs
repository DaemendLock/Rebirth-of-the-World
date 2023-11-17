using Core.Combat.Stats;
using Core.Combat.Units;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
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
            _value = SpellSerializer.DeserializeSpellValue(source);
        }

        public void ApplyEffect(CastEventData data, float modifyValue)
        {
            float originalDamage = _value.GetValue(data, modifyValue);

            Unit caster = data.Caster;

            if (caster != null)
            {
                originalDamage *= (1 + caster.EvaluateStat(UnitStat.DAMAGE_DONE).CalculatedValue) * caster.EvaluateVersalityMultiplyer();
            }

            HealthChangeEventData damage = new HealthChangeEventData(originalDamage, data.Caster, data.Target, data.Spell);

            DamageEvent @event = new DamageEvent(damage);

            caster.AmplifyDamage(@event);
            data.Target.TakeDamage(@event);
        }

        public float GetValue(float modifyValue) => _value.BaseValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.SCHOOL_DAMAGE);
            Serializer.SerializeEffect(_value, buffer);
        }
    }
}