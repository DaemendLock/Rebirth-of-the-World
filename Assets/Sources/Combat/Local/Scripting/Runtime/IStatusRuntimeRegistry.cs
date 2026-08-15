using Combat.Common.Primitives;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.IDK;

namespace Combat.Local.Scripting.Runtime
{
    public readonly struct StatusRuntime
    {
        public readonly DomainStatusContext Context;
        public readonly IStatusCapabilityProvider Container;

        public StatusRuntime(DomainStatusContext context, IStatusCapabilityProvider container)
        {
            Context = context;
            Container = container;
        }
    }

    public interface IStatusRuntimeRegistry
    {
        void Create(StatusId id, StatusRuntime value);
        void Remove(StatusId id);
        bool TryGet(StatusId id, out StatusRuntime runtime);
    }
}
