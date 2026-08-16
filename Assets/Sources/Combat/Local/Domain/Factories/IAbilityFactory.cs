using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.Factories
{
    public interface IAbilityFactory
    {
        SkillInfo Create(SkillId skill);
    }
}
