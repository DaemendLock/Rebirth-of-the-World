using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;

namespace Combat.Local.Domain.OutputPorts.Statuses
{
    public interface IStatusLifecycleHandler
    {
        void Apply(UnitId parent, StatusInstance status);
        void Expire(StatusId status);
        void Remove(StatusId status);
        void Reapply(StatusId status, float newDuration, AbilityKey? source);
    }
}
