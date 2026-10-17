using Combat.Common.Primitives;
using Combat.Domain.Entities.Units;

namespace Combat.Local.Application.Factories
{
    public sealed class ScaleEffectFactory
    {
        private int _nextId;

        public ScaleOverTimeEffect Create(UnitId owner, float rate) => new(new(_nextId++, owner), rate);
    }
}
