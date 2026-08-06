using Combat.Common.ValueObjects;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Scripting.Capabilities.Statuses;
using Combat.Local.Scripting.Runtime;

using System;

namespace Combat.Local.Scripting.Ports.Statuses
{
    public sealed class StatusPropertyDamageResultHandler : IDamageResultHandler
    {
        private readonly IStatusRuntimeRegistry _statusRuntimeRegistry;

        public StatusPropertyDamageResultHandler(IStatusRuntimeRegistry statusRuntimeRegistry)
        {
            _statusRuntimeRegistry = statusRuntimeRegistry;
        }

        public void HandleDamageDealth(ReadOnlySpan<StatusId> handlers, DamageResult @event)
        {
            foreach (StatusId statusId in handlers)
            {
                if (_statusRuntimeRegistry.TryGet(statusId, out var properties) == false)
                {
                    continue;
                }

                if (properties.TryGetProperty(out HandleOutgoingDamageCapability effect) == false)
                {
                    continue;
                }

                effect.Handle(@event);
            }
        }

        public void HandleDamageRecieved(ReadOnlySpan<StatusId> handlers, DamageResult @event)
        {
            foreach (StatusId statusId in handlers)
            {
                if (_statusRuntimeRegistry.TryGet(statusId, out var properties) == false)
                {
                    continue;
                }

                if (properties.TryGetProperty(out HandleIncomingDamageCapability effect) == false)
                {
                    continue;
                }

                effect.Handle(@event);
            }
        }
    }
}
