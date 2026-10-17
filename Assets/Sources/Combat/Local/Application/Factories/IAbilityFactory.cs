using Combat.Common.Primitives;
using Combat.Local.Application.Entities;

namespace Combat.Local.Application.Factories
{
    public interface IAbilityFactory
    {
        SkillInfo Create(SkillId skill);
    }
}
