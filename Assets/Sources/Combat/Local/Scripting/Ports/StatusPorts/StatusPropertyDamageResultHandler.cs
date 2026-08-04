using Combat.API;
using Combat.API.Adapters;
using Combat.API.API.IDK;
using Combat.API.API.Skills;
using Combat.API.Contexts;
using Combat.API.DTO;
using Combat.API.Statuses;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Scripting.Ports.StatusPorts
{
    public sealed class StatusPropertyDamageResultHandler : IDamageResultHandler
    {
        private readonly IStatusRuntimeRegistry _statusPropertyContainer;
        private readonly ICharacterApiAdapter _characterApiAdapter;
        private readonly IAbilityApiAdapter _abilityApiAdapter;

        public StatusPropertyDamageResultHandler(IStatusRuntimeRegistry statusPropertyContainer, ICharacterApiAdapter characterApiAdapter, IAbilityApiAdapter abilityApiAdapter)
        {
            _statusPropertyContainer = statusPropertyContainer;
            _characterApiAdapter = characterApiAdapter;
            _abilityApiAdapter = abilityApiAdapter;
        }

        public void HandleDamageDealth(ReadOnlySpan<StatusId> handlers, DamageResult @event)
        {
            Unit target = _characterApiAdapter.Adaptee(@event.Target);
            Unit attacker = @event.Attacker.HasValue ? _characterApiAdapter.Adaptee(@event.Attacker.Value) : null;
            AbilityApi abilityApi = @event.Skill.HasValue ? _abilityApiAdapter.Adaptee(@event.Skill.Value) : null;
            DamageRecord record = new(target, @event.OriginalDamage, @event.FinalDamage, @event.Flags, attacker, abilityApi);

            foreach (StatusId statusId in handlers)
            {
                if (_statusPropertyContainer.TryGet(statusId, out var properties) == false)
                {
                    continue;
                }

                if (properties.TryGetProperty(out IOutgoingHealDamageHandler effect) == false)
                {
                    continue;
                }

                effect.OnDealDamage(record);
            }

            GameEvent<DealDamageEventData> gameEvent = new(new());
        }

        public void HandleDamageRecieved(ReadOnlySpan<StatusId> handlers, DamageResult @event)
        {
            Unit target = _characterApiAdapter.Adaptee(@event.Target);
            Unit attacker = @event.Attacker.HasValue ? _characterApiAdapter.Adaptee(@event.Attacker.Value) : null;
            AbilityApi abilityApi = @event.Skill.HasValue ? _abilityApiAdapter.Adaptee(@event.Skill.Value) : null;
            DamageRecord record = new(target, @event.OriginalDamage, @event.FinalDamage, @event.Flags, attacker, abilityApi);

            foreach (StatusId statusId in handlers)
            {
                if (_statusPropertyContainer.TryGet(statusId, out var properties) == false)
                {
                    continue;
                }

                if (properties.TryGetProperty(out IIncomingHealDamageHandler effect) == false)
                {
                    continue;
                }

                effect.OnTakeDamage(record);
            }
        }
    }
}
