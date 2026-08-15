using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.OutputPorts.Statuses
{
    public interface IDamageResultHandler
    {
        void HandleDamageDealth(ReadOnlySpan<StatusId> handlers, DamageResult @event);
        void HandleDamageRecieved(ReadOnlySpan<StatusId> handlers, DamageResult @event);
    }
}
