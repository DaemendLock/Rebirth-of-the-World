using Combat.Common.ValueObjects;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Scripting.Runtime
{

    public interface IStatusRuntimeRegistry
    {
        void Create(StatusId id, IStatusPropertyContainer value);
        void Remove(StatusId id);
        bool TryGet(StatusId id, out IStatusPropertyContainer container);
    }
}
