using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Factories
{
    public class DamageInstanceFactory
    {
        public DamageInstance Create(EntityId targetId, float damage, EntityId? attackerId, DamageFlags flags, EventSource eventSource)
        {
            return new(targetId, damage, attackerId, flags, eventSource);
        }
    }
}
