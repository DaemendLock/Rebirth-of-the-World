using Combat.Common.ValueObjects;
using Combat.Local.API.IDK;

namespace Combat.API.API.IDK
{
    public interface IStatusRuntimeRegistry
    {
        void Create(StatusId id);
        void Remove(StatusId id);
        bool TryGet(StatusId id, out IStatusPropertyContainer container);
    }
}
