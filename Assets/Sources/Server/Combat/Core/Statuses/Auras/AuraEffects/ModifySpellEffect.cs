using Core.Combat.Abilities;
using Core.Combat.Statuses.Auras.AuraEffects;
using Core.Combat.Units;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.DataTypes;

namespace Core.Combat.Statuses.AuraEffects
{
    public class ModifySpellEffect : AuraEffect
    {
        private SpellId _spellId;
        private int _effect;
        private float _value;

        public ModifySpellEffect(Spell spell, int effect, float value)
        {
            _spellId = spell.Id;
            _effect = effect;
            _value = value;
        }

        public ModifySpellEffect(SpellId spellId, int effect, float value)
        {
            _spellId = spellId;
            _effect = effect;
            _value = value;
        }

        public ModifySpellEffect(BinaryReader source)
        {
            _spellId = (SpellId) source.ReadInt32();
            _effect = source.ReadInt32();
            _value = source.ReadSingle();
        }

        public void ApplyEffect(Unit target)
        {
            target.ModifySpellEffect(_spellId, _effect, _value);
        }

        public void RemoveEffect(Unit target)
        {
            target.ModifySpellEffect(_spellId, _effect, -_value);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) AuraEffectType.ModifySpellEffect);
            buffer.Write(_spellId);
            buffer.Write(_effect);
            buffer.Write(_value);
        }
    }
}
