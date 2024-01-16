using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.DataTypes;

namespace Core.Combat.Statuses.AuraEffects
{
    public class PeriodicallyTriggerSpell : AuraEffect
    {
        public readonly SpellId Spell;
        public readonly float Period;

        public PeriodicallyTriggerSpell(SpellId id, float perdiod)
        {
            Spell = id;
            Period = perdiod;
        }

        public PeriodicallyTriggerSpell(BinaryReader source)
        {
            Spell = (SpellId) source.ReadInt32();
            Period = source.ReadSingle();
        }

        public void ApplyEffect(Status status)
        {
            status.RegisterPeriodicEffect(this);
        }

        public void RemoveEffect(Status status) { }

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) AuraEffectType.PERIODICALLY_TRIGGER_SPELL);
            buffer.Write((int) Spell);
            buffer.Write(Period);
        }
    }
}
