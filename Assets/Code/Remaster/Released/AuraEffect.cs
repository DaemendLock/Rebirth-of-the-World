using Remaster.Events;
using Remaster.Stats;
using Remaster.Utils;
using System.IO;

namespace Remaster.AuraEffects
{
    public class ModStat : AuraEffect
    {
        private readonly UnitStat _stat;
        private readonly PercentModifiedValue _value;

        public ModStat(UnitStat stat, PercentModifiedValue value)
        {
            _stat = stat;
            _value = value;
        }

        public ModStat(BinaryReader source)
        {
            _stat = (UnitStat) source.ReadInt32();
            _value = new PercentModifiedValue(source.ReadSingle(), source.ReadSingle());
        }

        public void ApplyEffect(Status status)
        {
            status.Parent.ModifyStat(_stat, _value);
        }

        public void RemoveEffect(Status status)
        {
            status.Parent.ModifyStat(_stat, -_value);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().ToString());
            buffer.Write((int) _stat);
            buffer.Write(_value.BaseValue);
            buffer.Write(_value.Percent);
        }
    }

    public class ReactionCast : DynamicEffect
    {
        private readonly int _spell;

        public ReactionCast(int spellId, UnitAction action) : base(action)
        {
            _spell = spellId;
        }

        public ReactionCast(BinaryReader source) : base((UnitAction) source.ReadInt32())
        {
            _spell = source.ReadInt32();
        }

        public override void Serialize(BinaryWriter buffer)
        {
            buffer.Write(GetType().FullName);
            buffer.Write((int) Action);
            buffer.Write(_spell);
        }

        public override void Update(Status status, EventData data)
        {
            status.Parent.CastAbility(new EventData(data.Caster, data.Target, SpellLib.SpellLib.GetSpell(_spell)));
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
}
