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

    public interface IStatusRuntimeFactory
    {
        bool CanHandle(StatusType statusName);
        StatusRuntime Create(StatusId id, StatusType name, UnitId parent, AbilityKey? source);
    }
}
