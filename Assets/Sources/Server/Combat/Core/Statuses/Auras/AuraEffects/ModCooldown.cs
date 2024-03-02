using Core.Combat.Abilities;
using Core.Combat.Statuses.Auras.AuraEffects;
using Core.Combat.Units;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.DataTypes;

namespace Core.Combat.Statuses.AuraEffects
{
    public class ModCooldown : AuraEffect
    {
        private readonly SpellId _spellId;
        private readonly PercentModifiedValue _value;

        public ModCooldown(Spell spell, PercentModifiedValue value)
        {
            _spellId = spell.Id;
            _value = value;
        }

        public ModCooldown(SpellId spellId, PercentModifiedValue value)
        {
            _spellId = spellId;
            _value = value;
        }

        public ModCooldown(BinaryReader source)
        {
            _spellId = (SpellId) source.ReadInt32();
            _value = new PercentModifiedValue(source.ReadSingle(), source.ReadSingle());
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) AuraEffectType.ModifyCooldown);
            buffer.Write(_spellId);
            buffer.Write(_value.BaseValue);
            buffer.Write(_value.Percent);
        }

        public void ApplyEffect(Unit target)
        {
            target.ModifyCooldown(_spellId, _value);
        }

        public void RemoveEffect(Unit target)
        {
            target.ModifyCooldown(_spellId, -_value);
        }
    }
}
