using Remaster.Data.Serializer;
using Remaster.Events;
using Remaster.Interfaces;
using Remaster.SpellEffects;
using Remaster.Stats;
using Remaster.Utils;
using System;
using System.IO;

namespace Remaster.AuraEffects
{
    public interface AuraEffect : SerializableInterface
    {
        void ApplyEffect(Status status);
        void RemoveEffect(Status status);
    }

    public class ModCooldown : AuraEffect
    {
        private readonly int _spellId;
        private readonly PercentModifiedValue _value;
        private readonly PercentModifiedValue _removeValue;

        public ModCooldown(Spell spell, PercentModifiedValue value)
        {
            _spellId = spell.Id;
            _value = value;
            _removeValue = -value;
        }

        public ModCooldown(int spellId, PercentModifiedValue value)
        {
            _spellId = spellId;
            _value = value;
            _removeValue = -value;
        }

        public ModCooldown(BinaryReader source)
        {
            _spellId = source.ReadInt32();
            _value = new PercentModifiedValue(source.ReadSingle(), source.ReadSingle());
            _removeValue = -_value;
        }

        public void ApplyEffect(Status status)
        {
            throw new NotImplementedException();
        }

        public void RemoveEffect(Status status)
        {
            throw new NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().Name);
            buffer.Write(_spellId);
            buffer.Write(_value.BaseValue);
            buffer.Write(_value.Percent);
        }
    }

    public class Immunity : AuraEffect
    {
        private Mechanic _mechanic;

        public void ApplyEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            throw new NotImplementedException();
        }
    }

    public class PeriodicalTriggerSpell : AuraEffect
    {
        public void ApplyEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            throw new NotImplementedException();
        }
    }

    public class ProcTriggerSpell : AuraEffect
    {
        public void ApplyEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            throw new NotImplementedException();
        }
    }

    public class ModHealthRegen : AuraEffect
    {
        public void ApplyEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            throw new NotImplementedException();
        }
    }

    public class AbsorbDamage : AuraEffect
    {
        private ValueSource _health;
        private SchoolType _type;

        public AbsorbDamage(ValueSource health, SchoolType type)
        {
            _health = health;
            _type = type;
        }

        public void ApplyEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            throw new NotImplementedException();
        }
    }

    #region CroudControl
    public class Stun : AuraEffect
    {
        public void ApplyEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            throw new NotImplementedException();
        }
    }

    public class Fear : AuraEffect
    {
        public void ApplyEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            throw new NotImplementedException();
        }
    }

    public class Root : AuraEffect
    {
        public void ApplyEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEffect(Status status)
        {
            throw new System.NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            throw new NotImplementedException();
        }
    }
    #endregion

    public abstract class DynamicEffect : AuraEffect
    {
        protected readonly UnitAction Action;

        public DynamicEffect(UnitAction action)
        {
            Action = action;
        }

        public void ApplyEffect(Status status)
        {
            status.RegisterDynamicEffect(this, Action);
        }

        public void RemoveEffect(Status status)
        {
        }

        public abstract void Serialize(BinaryWriter buffer);

        public abstract void Update(Status status, EventData data);
    }

    public interface DynamicModStat : AuraEffect
    {
        public UnitStat Stat { get; }

        public PercentModifiedValue Evaluate(Status status);
    }
}
