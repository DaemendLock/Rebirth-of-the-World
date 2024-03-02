using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Statuses.Auras.AuraReactions;
using Core.Combat.Utils.Serialization;
using System.IO;
using Utils.DataTypes;

namespace Core.Combat.Statuses.AuraEffects
{
    public class PeriodicallyTriggerSpell : AuraReaction
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

        public void Serialize(BinaryWriter buffer)
        {
            buffer.Write((byte) AuraEffectType.PERIODICALLY_TRIGGER_SPELL);
            buffer.Write((int) Spell);
            buffer.Write(Period);
        }

        public IActionRecordContainer Trigger(ActionRecord record)
        {
            throw new System.NotImplementedException();
        }
    }
}
