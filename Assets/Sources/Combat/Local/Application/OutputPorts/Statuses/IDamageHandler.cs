using Combat.Common.Primitives;
using Combat.Local.Application.DTO;

using System;
using System.Collections.Generic;

namespace Combat.Local.Application.OutputPorts.Statuses
{
    public interface IDamageResultHandler
    {
        void NotifyDamageDealth(DamageRecord @event, IEnumerable<StatusId> handlers);
        void NotifyDamageRecieved(DamageRecord @event, IEnumerable<StatusId> handlers);
    }
}
