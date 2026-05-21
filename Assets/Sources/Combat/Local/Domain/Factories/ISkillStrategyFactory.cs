using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Factories
{
    public interface ISkillStrategyFactory
    {
        bool CanHandle(SkillId skillId);

        IAbilityPropertyContainer Create(UnitId? owner, SkillId skillId);
    }
}
