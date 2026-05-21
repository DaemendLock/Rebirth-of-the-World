using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Entities.Statuses
{
    public interface IModifyParentOutgoingHealingStrategy
    {
        HealingModification GetModification(HealingInstance instance);
    }
}
