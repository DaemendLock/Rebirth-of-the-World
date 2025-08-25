using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Infrastructure.Factories;

namespace Testing.Local.Temp.Factories
{

    public class StatusFactory : IStatusFactory
    {
        private int _nextId = 0;
        
        public Status Create(StatusName name, EntityId parentId, EntityId caster, SkillId source, float duration, int stackCount)
        {
            return new(new(_nextId++), parentId, name, caster, source, stackCount, new(0, duration));
        }
    }
}
