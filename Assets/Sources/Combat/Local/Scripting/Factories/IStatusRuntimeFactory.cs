using Combat.Common.Primitives;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.Factories
{
    public interface IStatusRuntimeFactory
    {
        bool CanHandle(StatusType statusName);
        StatusRuntime Create(UnitId parent, StatusId id, StatusType name, AbilityKey? source);
    }
}
