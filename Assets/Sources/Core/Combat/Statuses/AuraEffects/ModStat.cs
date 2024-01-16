using Core.Combat.Utils.ValueSources;
using System.IO;
using Utils.DataStructure;
using Utils.Serializer;

namespace Core.Combat.Auras.AuraEffects
{
    public class ModStat : AuraEffect
    {
        private readonly UnitStat _stat;
        private readonly AuraValueSource _value;
        private readonly bool _isPercent;

        public ModStat(UnitStat stat, AuraValueSource value, bool percent)
        {
            _stat = stat;
            _value = value;
            _isPercent = percent;
        }

        public bool IsPercent => _isPercent;
        public UnitStat Stat => _stat;

        public ModStat(BinaryReader source)
        {
            _stat = (UnitStat) source.ReadInt32();
            _value = SpellSerializer.DeserializeAuraValue(source);
            _isPercent = source.ReadBoolean();
        }

        public void ApplyEffect(Status status)
        {
            status.RegisterStatModification(this);
        }

        public void RemoveEffect(Status status)
        {
            status.RemoveStatModification(this);
        }

        public float Evaluate(Status status)
        {
            return _value.Evaluate(status);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) AuraEffectType.MODIFY_STAT);
            buffer.Write((int) _stat);
            _value.Serialize(buffer);
            buffer.Write(_isPercent);

        }
    }
}
