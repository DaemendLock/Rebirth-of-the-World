using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.OutputPorts.Statuses
{
    public interface IStatusLifecycleHandler
    {
        void Apply(Status status);
        void Cleanup(StatusId status);
    }
}
