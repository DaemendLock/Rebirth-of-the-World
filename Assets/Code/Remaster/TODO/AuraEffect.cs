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
        private readonly Spell _spell;
        private readonly PercentModifiedValue _value;
        private readonly PercentModifiedValue _removeValue;

        public ModCooldown(Spell spell, PercentModifiedValue value)
        {
            _spell = spell;
            _value = value;
            _removeValue = -value;
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
            throw new NotImplementedException();
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

    public class ModifySpellEffect : AuraEffect
    {
        private int _spellId;
        private int _effect;
        private float _value;

        public ModifySpellEffect(Spell spell, int effect, float value)
        {
            _spellId = spell.Id;
            _effect = effect;
            _value = value;
        }

        public ModifySpellEffect(int spellId, int effect, float value)
        {
            _spellId = spellId;
            _effect = effect;
            _value = value;
        }

        public ModifySpellEffect(BinaryReader source)
        {
            _spellId = source.ReadInt32();
            _effect = source.ReadInt32();
            _value = source.ReadSingle();
        }

        public void ApplyEffect(Status status)
        {
            status.Caster?.ModifySpellEffect(_spellId, _effect, _value);
        }

        public void RemoveEffect(Status status)
        {
            status.Caster?.ModifySpellEffect(_spellId, _effect, -_value);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().ToString());
            buffer.Write(_spellId);
            buffer.Write(_effect);
            buffer.Write(_value);
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
