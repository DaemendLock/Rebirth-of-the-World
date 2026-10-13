using Combat.Common.Primitives;
using Combat.Local.Domain.ValueObjects;

using System;
using System.Collections.Generic;

namespace Combat.Local.Domain.OutputPorts.Statuses
{
    public interface IDamageResultHandler
    {
        void NotifyDamageDealth(DamageResult @event, IReadOnlyCollection<StatusId> handlers);
        void NotifyDamageRecieved(DamageResult @event, IReadOnlyCollection<StatusId> handlers);
    }
}
