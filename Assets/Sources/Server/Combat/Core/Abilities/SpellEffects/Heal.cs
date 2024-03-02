using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Units;
using Core.Combat.Units.Interfaces;
using Core.Combat.Utils.HealthChangeProcessing;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.ByteHelper;
using Utils.DataTypes;
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

        public Heal(ByteReader source)
        {
            _value = SpellSerializer.DeserializeSpellValue(source);
        }

        public ActionRecord ApplyEffect(EffectApplicationData data, float modification)
        {
            float healing = _value.GetValue(data.Caster, data.Target, modification);

            HealthChangeEvent @event = new(new(healing, data.Caster, data.Target, data.Source));

            data.Caster.ApplyOutcomeHealingModification(@event);
            data.Target.ApplyIncomeHealingModification(@event);

            return data.Target.Heal(@event);
        }

        private class HealEvent
        {
            public Unit target;
            public PercentModifiedValue _value;
            public bool missed;
            public bool parried;
            public bool blocked;

            public HealEvent(Unit target, float initialValue, StatsOwner attackerStats, SpellId source)
            {
                this.target = target;
                _value = new PercentModifiedValue(initialValue, 100);
            }
        }

        public float GetValue(float modifyValue) => _value.BaseValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) SpellEffectType.HEAL);
            Serializer.SerializeEffect(_value, buffer);
        }
    }
}