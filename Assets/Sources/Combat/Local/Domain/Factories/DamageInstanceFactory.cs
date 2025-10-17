using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Factories
{
    public class DamageInstanceFactory
    {
        private int _nextId = 0;

        public DamageInstance Create(EntityId targetId, float damage, EntityId? attackerId, DamageFlags flags, EventSource eventSource)
        {
            DamageInstanceId id = new(_nextId++);
            return new(id, targetId, damage, attackerId, flags, eventSource);
        }
    }
}
