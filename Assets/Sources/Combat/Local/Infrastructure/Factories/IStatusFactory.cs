using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Infrastructure.Factories
{
    public interface IStatusFactory
    {
        Status Create(StatusName name, EntityId parent, EntityId caster, SkillId source, float duration, int stackCount);
    }
}
