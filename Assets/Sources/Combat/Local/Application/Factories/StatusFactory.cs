using Combat.Common.Primitives;
using Combat.Domain.Entities;

namespace Combat.Local.Application.Factories
{
    public class StatusFactory
    {
        private int _nextId = 0;

        public StatusInstance Create(StatusType name, AbilityKey? source) =>
            new(new StatusId(_nextId++), name, default, 1, source);
    }
}
