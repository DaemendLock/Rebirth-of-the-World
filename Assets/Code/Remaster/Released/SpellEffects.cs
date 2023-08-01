using Remaster.AuraEffects;
using Remaster.Data.Serializer;
using Remaster.Events;
using Remaster.Interfaces;
using Remaster.Stats;
using System.IO;

namespace Remaster.SpellEffects
{
    public interface SpellEffect : SerializableInterface
    {
        public float GetValue(float modifyValue);

        public void ApplyEffect(EventData data, float modifyValue);
    }

    public class Dummy : SpellEffect
    {
        private readonly float _value;

        public Dummy(float value)
        {
            _value = value;
        }

        public Dummy(BinaryReader source)
        {
            _value = source.ReadSingle();
        }

        public float GetValue(float modifyValue) => _value + modifyValue;

        public void ApplyEffect(EventData data, float modifyValue)
        {

        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().ToString());
            buffer.Write(_value);
        }
    }

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

            HealthChangeEventData healing = new HealthChangeEventData(originalHealing, data.Caster, data.Target, data.Spell);

            data.Target.Heal(healing);
        }

        public float GetValue(float modifyValue) => _value.BaseValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().ToString());
            Serializer.SerializeEffect(_value, buffer);
        }
    }

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

    public class ReduceCooldown : SpellEffect
    {
        private readonly int _spell;
        private readonly float _duration;

        public ReduceCooldown(Spell spell, float duration)
        {
            _spell = spell.Id;
            _duration = duration;
        }

        public ReduceCooldown(int spellId, float duration)
        {
            _spell = spellId;
            _duration = duration;
        }

        public ReduceCooldown(BinaryReader source)
        {
            _spell = source.ReadInt32();
            _duration = source.ReadSingle();
        }

        public void ApplyEffect(EventData data, float modifyValue)
        {
            data.Caster.FindAbility(SpellLib.SpellLib.GetSpell(_spell))?.ReduceCooldown(_duration + modifyValue);
        }

        public float GetValue(float modifyValue)
        {
            return _duration + modifyValue;
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().ToString());
            buffer.Write(_spell);
            buffer.Write(_duration);
        }
    }

    public class GiveResource : SpellEffect
    {
        private float _value;
        private ResourceType _type;

        public GiveResource(float value, ResourceType type)
        {
            _value = value;
            _type = type;
        }

        public GiveResource(BinaryReader source)
        {
            _value = source.ReadSingle();
            _type = (ResourceType) source.ReadUInt16();
        }

        public void ApplyEffect(EventData data, float modifyValue)
        {
            data.Target.GiveResource(_type, GetValue(modifyValue));
        }

        public float GetValue(float modifyValue) => _value + modifyValue;

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().ToString());
            buffer.Write(_value);
            buffer.Write((ushort) _type);
        }
    }

    public class TriggerSpell : SpellEffect
    {
        private readonly int _spell;

        public TriggerSpell(Spell spell)
        {
            _spell = spell.Id;
        }

        public TriggerSpell(int spellId)
        {
            _spell = spellId;
        }
        public TriggerSpell(BinaryReader source)
        {
            _spell = source.ReadInt32();
        }

        public void ApplyEffect(EventData data, float modifyValue)
        {
            data.Caster.CastAbility(new EventData(data.Caster, data.Target, SpellLib.SpellLib.GetSpell(_spell)));
        }

        public float GetValue(float modifyValue)
        {
            return _spell;
        }
        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().ToString());
            buffer.Write(_spell);
        }
    }

    public class ApplyAura : SpellEffect
    {
        private readonly AuraEffect _effect;
        private readonly int _value;

        public ApplyAura(AuraEffect effect, int value = 0)
        {
            _effect = effect;
            _value = value;
        }

        public ApplyAura(BinaryReader source)
        {
            _effect = Serializer.Deserialize<AuraEffect>(source);
            _value = source.ReadInt32();
        }

        public float GetValue(float modifyValue)
        {
            return modifyValue + _value;
        }

        public void ApplyEffect(EventData data, float modifyValue)
        {
            if (modifyValue + _value < 0)
            {
                return;
            }

            data.Target.ApplyAura(data, _effect);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().ToString());
            Serializer.SerializeEffect(_effect, buffer);
            buffer.Write(_value);
        }
    }
}
