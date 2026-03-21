using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Factories
{
    public interface ISkillStrategyFactory
    {
        bool CanHandle(SkillId skillId);

        ISkillStrategy Create(SkillId skillId, EntityId? owner);
    }

    public interface ISkillFactory
    {
        void RegisterStrategyFactory(ISkillStrategyFactory factory);

        Skill Create(SkillId skill, EntityId? owner);
    }
}
