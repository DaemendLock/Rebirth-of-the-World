using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils;
using Core.Combat.Utils.HealthChangeProcessing;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.ByteHelper;
using Utils.Serializer;

namespace Core.Combat.Abilities.SpellEffects
{

    public class SchoolDamage : SpellEffect
    {
        private readonly SpellValueSource _value;

        public SchoolDamage(SpellValueSource damage)
        {
            _value = damage;
        }

        public SchoolDamage(ByteReader source)
        {
            _value = SpellSerializer.DeserializeSpellValue(source);
        }

        public ActionRecord ApplyEffect(EffectApplicationData data, float modification)
        {
            float originalDamage = _value.GetValue(data.Caster, data.Target, modification);

            Unit caster = data.Caster;
            Unit target = data.Target;

            HealthChangeEventData damage = new HealthChangeEventData(originalDamage, data.Caster, data.Target, data.Source);

            HealthChangeEvent @event = new HealthChangeEvent(damage);

            caster.ApplyOutcomeDamageModification(@event);
            target.ApplyIncomeDamageModification(@event);

            return target.TakeDamage(@event);
        }

        public float GetValue(float modifyValue) => _value.BaseValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.SCHOOL_DAMAGE);
            Serializer.SerializeEffect(_value, buffer);
        }
    }
}