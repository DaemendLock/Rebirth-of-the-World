using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Factories
{
    public class UnitIdFactory
    {
        private int _nextId = 0;

        public UnitIdFactory() { }

        public EntityId GetId() => new(_nextId++);
    }
}
