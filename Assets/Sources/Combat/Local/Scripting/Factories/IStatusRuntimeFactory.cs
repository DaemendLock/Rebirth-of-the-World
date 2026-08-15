using Combat.Common.ValueObjects;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.Factories
{
    public interface IStatusRuntimeFactory
    {
        bool CanHandle(StatusType statusName);
        StatusRuntime Create(StatusId id, StatusType name, UnitId parent, AbilityKey? source);
    }
}
