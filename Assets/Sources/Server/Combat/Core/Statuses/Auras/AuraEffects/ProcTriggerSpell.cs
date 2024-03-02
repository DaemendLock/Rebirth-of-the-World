using Core.Combat.Abilities.ActionRecords;
using Core.Combat.Statuses.Auras.AuraReactions;
using System;
using System.IO;

namespace Core.Combat.Statuses.AuraEffects
{
    public class ProcTriggerSpell : AuraReaction
    {
        public ProcTriggerSpell(BinaryReader source)
        {

        }

        public IActionRecordContainer Trigger(ActionRecord record)
        {
            throw new NotImplementedException();
        }

        public void Serialize(BinaryWriter buffer)
        {
            throw new NotImplementedException();
        }
    }
}
