using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Factories
{
    public interface IAbilityFactory
    {
        Ability Create(SkillId skill, UnitId? owner);
    }
}
