using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Factories
{
    public class StatusFactory
    {
        private int _nextId = 0;

        public Status Create(StatusType name, UnitId parentId, float duration, int stackCount, AbilityKey? source) =>
            new(new StatusId(_nextId++), parentId, name, source, stackCount, new(0, duration));
    }
}
