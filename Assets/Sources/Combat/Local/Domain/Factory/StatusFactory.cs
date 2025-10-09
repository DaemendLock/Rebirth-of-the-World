using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.Factories
{
    public class StatusFactory
    {
        private int _nextId = 0;

        public Status Create(StatusName name, EntityId parentId, float duration, int stackCount, EventSource source)
        {
            StatusId nextId = new(_nextId++);

            return new(nextId, parentId, name, source, stackCount, new(0, duration));
        }
    }
}
