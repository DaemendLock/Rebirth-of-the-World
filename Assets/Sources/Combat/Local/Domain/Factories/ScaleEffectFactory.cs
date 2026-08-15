using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Units;

namespace Combat.Local.Domain.Factories
{
    public sealed class ScaleEffectFactory
    {
        private int _nextId;

        public ScaleOverTimeEffect Create(UnitId owner, float rate) => new(new(_nextId++, owner), rate);
    }
}
