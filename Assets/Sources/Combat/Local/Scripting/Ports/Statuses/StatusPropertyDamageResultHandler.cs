using Combat.API.Contexts;
using Combat.API.Events;
using Combat.Common.Primitives;
using Combat.Local.Application.DTO;
using Combat.Local.Application.OutputPorts.Statuses;
using Combat.Local.Scripting.Capabilities.Statuses;
using Combat.Local.Scripting.Runtime;

using System.Collections.Generic;

namespace Combat.Local.Scripting.Ports.Statuses
{
    public sealed class StatusPropertyDamageResultHandler : IDamageResultHandler
    {
        private readonly IStatusRuntimeRegistry _statusRuntimeRegistry;
        private readonly IEventContext _eventContext;

        public StatusPropertyDamageResultHandler(IStatusRuntimeRegistry statusRuntimeRegistry, IEventContext eventContext)
        {
            _statusRuntimeRegistry = statusRuntimeRegistry;
            _eventContext = eventContext;
        }

        public void NotifyDamageDealth(DamageRecord @event, IEnumerable<StatusId> handlers)
        {
            foreach (StatusId statusId in handlers)
            {
                if (_statusRuntimeRegistry.TryGet(statusId, out StatusRuntime runtime) == false)
                {
                    continue;
                }

                IStatusHandleOutgoingDamageCapability effect = runtime.Container.GetCapability<IStatusHandleOutgoingDamageCapability>();

                if (effect == null)
                {
                    continue;
                }

                effect.Handle(runtime.Context, @event);
            }

            _eventContext.Publish(new GameEvent<DealDamageEventData>(new(@event.FinalDamage)));
        }

        public void NotifyDamageRecieved(DamageRecord @event, IEnumerable<StatusId> handlers)
        {
            foreach (StatusId statusId in handlers)
            {
                if (_statusRuntimeRegistry.TryGet(statusId, out StatusRuntime runtime) == false)
                {
                    continue;
                }

                IStatusHandleIncomingDamageCapability effect = runtime.Container.GetCapability<IStatusHandleIncomingDamageCapability>();

                if (effect == null)
                {
                    continue;
                }

                effect.Handle(runtime.Context, @event);
            }

            _eventContext.Publish(new GameEvent<TakeDamageEventData>(new(@event.FinalDamage)));
        }
    }
}
