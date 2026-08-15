using Combat.Common.ValueObjects;
using Combat.Local.Scripting.IDK;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.Factories
{
    public interface ISkillRuntimeFactory
    {
        bool CanHandle(SkillId skillId);
        SkillRuntime Create(UnitId? owner, SkillId skillId);
    }

    public interface IStatusPropertyContainerFactory
    {
        bool CanHandle(StatusType statusName);
        IStatusPropertyContainer Create(StatusId id, StatusType name, UnitId parent, AbilityKey? source);
    }
}
