using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Factories
{
    public interface ISkillFactory
    {
        void RegisterStrategyFactory(ISkillStrategyFactory factory);

        Skill Create(SkillId skill, EntityId? owner);
    }
}
