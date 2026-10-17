using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Factories
{
    public class StatusFactory
    {
        private int _nextId = 0;

        public StatusInstance Create(StatusType name, AbilityKey? source) =>
            new(new StatusId(_nextId++), name, default, 1, source);
    }
}
