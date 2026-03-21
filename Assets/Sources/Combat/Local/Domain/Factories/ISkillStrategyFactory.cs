using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Factories
{
    public interface ISkillStrategyFactory
    {
        bool CanHandle(SkillId skillId);

        ISkillStrategy Create(SkillId skillId, EntityId? owner);
    }
}
