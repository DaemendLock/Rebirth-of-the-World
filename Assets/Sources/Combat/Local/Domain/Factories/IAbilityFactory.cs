using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Factories
{
    public interface IAbilityFactory
    {
        void RegisterStrategyFactory(ISkillStrategyFactory factory);

        Ability Create(SkillId skill, UnitId? owner);
    }
}
