using Combat.API.API.IDK;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts.Statuses;

namespace Combat.Local.Scripting.Ports.StatusPorts
{
    public sealed class PropertyStatusLifecycleHandler : IStatusLifecycleHandler
    {
        private readonly IStatusRuntimeRegistry _statusRuntimeRegistry;

        public void Apply(Status status)
        {

        }

        public void Cleanup(StatusId id)
        {
            _statusRuntimeRegistry.Remove(id);
        }
    }
}
