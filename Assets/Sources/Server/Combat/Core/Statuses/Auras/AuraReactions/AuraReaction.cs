using Core.Combat.Abilities.ActionRecords;

namespace Core.Combat.Statuses.Auras.AuraReactions
{
    internal interface AuraReaction
    {
        public IActionRecordContainer Trigger(ActionRecord record);
    }
}
