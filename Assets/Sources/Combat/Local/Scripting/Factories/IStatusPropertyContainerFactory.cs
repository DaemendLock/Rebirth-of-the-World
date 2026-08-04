using Combat.API;
using Combat.Common.ValueObjects;
using Combat.Local.API.IDK;

namespace Combat.Local.Scripting.Factories
{
    public interface ISkillPropertyContainerFactory
    {
        bool CanHandle(SkillId skillId);

        IAbilityPropertyContainer Create(UnitId? owner, SkillId skillId);
    }

    public interface IStatusPropertyContainerFactory
    {
        bool CanHandle(StatusType statusName);

        IStatusPropertyContainer Create(StatusId id, StatusType name, UnitId parent, AbilityKey? source);
    }
}
