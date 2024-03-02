using Core.Combat.Statuses.Auras.AuraEffects;
using Core.Combat.Units;
using Core.Combat.Utils.Serialization;
using Core.Combat.Utils.ValueSources;
using System.IO;
using Utils.DataStructure;

namespace Core.Combat.Statuses.AuraEffects
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
            _value = AuraSerializer.DeserializeAuraValue(source);
            _isPercent = source.ReadBoolean();
        }

        public void ApplyEffect(Unit target)
        {
            throw new System.NotImplementedException();
        }

        public void RemoveEffect(Unit target)
        {
            throw new System.NotImplementedException();
        }

        public float Evaluate(Status status)
        {
            return _value.Evaluate(status);
        }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) AuraEffectType.ModifyStat);
            buffer.Write((int) _stat);
            _value.Serialize(buffer);
            buffer.Write(_isPercent);
        }
    }
}
