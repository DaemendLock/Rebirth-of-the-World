using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.DataStructure;
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

        //public ActionRecord ApplyEffect(Unit target, SpellEffectValueProvider values)
        //{
        //    float originalDamage = values.GetValue(this);

        //    Unit caster = values.Caster;

        //    if (caster != null)
        //    {
        //        originalDamage *= (1 + caster.EvaluateStat(UnitStat.DAMAGE_DONE).CalculatedValue) * caster.EvaluateVersalityMultiplyer();
        //    }

        //    HealthChangeEventData damage = new HealthChangeEventData(originalDamage, data.Caster, data.Target, data.Spell);

        //    DamageEvent @event = new DamageEvent(damage);

        //    caster.AmplifyDamage(@event);
        //    target.TakeDamage(@event);

        //    return new DamageActionRecord(target.Id, values.Source);
        //}

        public ActionRecord ApplyEffect(Unit caster, Unit target, float modification)
        {
            throw new System.NotImplementedException();
        }

        public float GetValue(float modifyValue) => _value.BaseValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.SCHOOL_DAMAGE);
            Serializer.SerializeEffect(_value, buffer);
        }
    }
}