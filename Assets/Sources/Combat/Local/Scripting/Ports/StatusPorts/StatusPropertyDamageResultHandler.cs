using Combat.API.API.Skills;
using Combat.API.Contexts;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Statuses;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Scripting.Ports.StatusPorts
{
    public sealed class StatusPropertyDamageResultHandler : IDamageResultHandler
    {
        private readonly IStatusRepository _statusRepository;

        public void HandleDamageDealth(ReadOnlySpan<StatusId> handlers, DamageResult @event)
        {
            foreach (StatusId statusId in handlers)
            {
                if (_statusRepository.TryGet(statusId, out Status status) == false)
                {
                    continue;
                }

                if (status.Properties.TryGetProperty(out ITakeDamageEffectStrategy effect) == false)
                {
                    continue;
                }

                effect.HandleDamage(@event);
            }

            GameEvent<DealDamageEventData> gameEvent = new(new());
        }

        public void HandleDamageRecieved(ReadOnlySpan<StatusId> handlers, DamageResult @event)
        {
            foreach (var statusId in handlers)
            {
                if (_statusRepository.TryGet(statusId, out Status status) == false)
                {
                    continue;
                }

                if (status.Properties.TryGetProperty(out IDealDamageEffectStrategy effect) == false)
                {
                    continue;
                }

                effect.HandleDamage(@event);
            }
        }
    }
}
