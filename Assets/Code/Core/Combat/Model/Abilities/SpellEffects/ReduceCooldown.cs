using Core.Combat.Utils;
using System.IO;

namespace Core.Combat.Abilities.SpellEffects
{
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
            data.Caster.FindAbility(Core.Data.SpellLib.SpellLib.GetSpell(_spell))?.ReduceCooldown(_duration + modifyValue);
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
}