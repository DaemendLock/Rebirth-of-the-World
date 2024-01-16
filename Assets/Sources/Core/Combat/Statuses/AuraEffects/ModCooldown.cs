using Core.Combat.Abilities;
using Core.Combat.Utils.Serialization;
using System;
using System.IO;
using Utils.DataTypes;

namespace Core.Combat.Statuses.AuraEffects
{
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
            buffer.Write((byte) AuraEffectType.MODIFY_COOLDOWN);
            buffer.Write(_spellId);
            buffer.Write(_value.BaseValue);
            buffer.Write(_value.Percent);
        }
    }
}
