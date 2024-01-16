using Core.Combat.Units;
using Core.Combat.Utils;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.DataStructure;
using Utils.Serializer;

namespace Core.Combat.Abilities.SpellEffects
{
    public class Heal : SpellEffect
    {
        private readonly SpellValueSource _value;

        public Heal(SpellValueSource healing)
        {
            _value = healing;
        }

        public Heal(BinaryReader source)
        {
            _value = SpellSerializer.DeserializeSpellValue(source);
        }

        public void ApplyEffect(CastEventData data, float modifyValue)
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
            buffer.Write((byte) SpellEffectType.HEAL);
            Serializer.SerializeEffect(_value, buffer);
        }
    }
}