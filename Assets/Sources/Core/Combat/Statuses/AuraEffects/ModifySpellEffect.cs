using Core.Combat.Abilities;
using System.IO;
using Utils.DataTypes;
using Utils.Serializer;

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

        public void ApplyEffect(Status status)
        {
            status.Parent.ModifySpellEffect(_spellId, _effect, _value);
        }

        public void RemoveEffect(Status status)
        {
            status.Parent.ModifySpellEffect(_spellId, _effect, -_value);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) AuraEffectType.MODIFY_SPELL_EFFECT);
            buffer.Write(_spellId);
            buffer.Write(_effect);
            buffer.Write(_value);
        }
    }
}
