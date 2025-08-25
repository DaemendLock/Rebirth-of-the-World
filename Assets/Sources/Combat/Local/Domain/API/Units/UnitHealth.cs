using System;
using System.Collections.Generic;

using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.API.Statuses;
using Combat.Local.Domain.API.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Services;

namespace Combat.Local.Domain.API
{
    public sealed partial class Unit
    {
        private readonly HealthService _healthService;
        private readonly IKillReviveService _killReviveService;

        public bool Alive => _killReviveService.IsAlive(Id);

        public float CurrentHealth
        {
            get => _healthService.GetHealth(Id).CurrentHealth;
            set => _healthService.SetCurrentHealth(Id, value);
        }

        public float MaxHealth => _healthService.GetHealth(Id).MaxHealth;

        public float ApplyDamage(DamageData data)
        {
            IEnumerable<StatusApi> defenderEffects = _statusLookupService.FindStatusesOnUnit(Id);
            IEnumerable<StatusApi> attackerEffects = data.Attacker != null ?
                _statusLookupService.FindStatusesOnUnit(data.Attacker.Id) : Array.Empty<StatusApi>();

            DamageInstance instance = CreateDamageInstance(this, data, defenderEffects, attackerEffects);
            Health newHealth = _healthService.ApplyDamage(instance.Target.Id, instance.GetCurrentDamage(), instance.Flags.HasFlag(DamageFlags.NonLethal));

            if (instance.Flags.HasFlag(DamageFlags.NonReactable) == false)
            {
                HandleDamageEvent(instance, defenderEffects, attackerEffects);
            }

            if (newHealth.CurrentHealth <= 0 && Alive)
            {
                Kill(new(data.Attacker, data.Source, KillFlags.None));
            }

            return newHealth.CurrentHealth;
        }

        public float ApplyHealing(HealingData data)
        {
            IEnumerable<StatusApi> healeeEffects = _statusLookupService.FindStatusesOnUnit(Id);
            IEnumerable<StatusApi> healerEffects = _statusLookupService.FindStatusesOnUnit(data.Healer.Id);

            HealingInstance instance = CreateHealingInstance(this, data, healeeEffects, healerEffects);
            Health newHealth = _healthService.ApplyHealing(instance.Target.Id, instance.GetCurrentHealing());

            if (instance.Flags.HasFlag(HealingFlags.NonReactable) == false)
            {
                HandleHealingEvent(instance, healeeEffects, healerEffects);
            }

            if (instance.Flags.HasFlag(HealingFlags.CanRevive) && newHealth.CurrentHealth > 0)
            {
                Revive(new(data.Healer, data.Source, ReviveFlags.Healed));
            }

            return newHealth.CurrentHealth;
        }

        public void Kill(KillData data)
        {
            if (Alive == false)
            {
                return;
            }

            _killReviveService.Kill(Id);

            IEnumerable<StatusApi> effects = _statusLookupService.FindStatusesOnUnit(Id);
        }

        public void Revive(ReviveData data)
        {
            if (Alive)
            {
                return;
            }

            _killReviveService.Revive(Id);

            IEnumerable<StatusApi> effects = _statusLookupService.FindStatusesOnUnit(Id);
        }

        private static HealingInstance CreateHealingInstance(Unit target, HealingData data, IEnumerable<StatusApi> healeeEffects, IEnumerable<StatusApi> healerEffects)
        {
            HealingInstance instance = new(target, data.Healer, data.Source, data.Healing, data.Flags);

            foreach (StatusApi effect in healerEffects)
            {
                if (effect is not IOutgoingHealDamageModifier modifier)
                {
                    continue;
                }

                instance.BaseHealing += modifier.GetBonusHealingDealth(instance);
                instance.HealingPercent += modifier.GetBonusHealingDealthPercent(instance);
                instance.Flags |= modifier.GetHealingFlagMask(instance);
            }

            foreach (StatusApi effect in healeeEffects)
            {
                if (effect is not IIncomingHealDamageModifier modifier)
                {
                    continue;
                }

                instance.BaseHealing += modifier.GetBonusHealingRecived(instance);
                instance.HealingPercent += modifier.GetBonusHealingRecivedPercent(instance);
                instance.Flags |= modifier.GetHealingFlagMask(instance);
            }

            return instance;
        }

        private static void HandleHealingEvent(HealingInstance instance, IEnumerable<StatusApi> healeeEffects, IEnumerable<StatusApi> healerEffects)
        {
            HealingRecord @event = new(instance);

            foreach (StatusApi effect in healerEffects)
            {
                if (effect is not IOutgoingHealDamageHandler modifier)
                {
                    continue;
                }

                modifier.OnDealHealing(@event);
            }

            foreach (StatusApi effect in healeeEffects)
            {
                if (effect is not IIncomingHealDamageHandler modifier)
                {
                    continue;
                }

                modifier.OnTakeHealing(@event);
            }
        }

        private static DamageInstance CreateDamageInstance(Unit target, DamageData data, IEnumerable<StatusApi> defenderEffects, IEnumerable<StatusApi> attackerEffects)
        {
            DamageInstance instance = new(target, data.Attacker, data.Source, data.Damage, data.Flags);

            //_api.GetDamageModifications(instance);
            IIncomingHealDamageModifier incomingHalDamageModifier;
            IOutgoingHealDamageModifier outgoingHealDamageModifier;

            foreach (StatusApi effect in attackerEffects)
            {
                if (effect.TryGetProperty(out outgoingHealDamageModifier) == false)
                {
                    continue;
                }

                instance.BaseDamage += outgoingHealDamageModifier.GetBonusDamageDealth(instance);
                instance.DamagePercent += outgoingHealDamageModifier.GetBonusDamageDealthPercent(instance);
                instance.Flags |= outgoingHealDamageModifier.GetDamageFlagMask(instance);
            }

            foreach (StatusApi effect in defenderEffects)
            {
                if (effect.TryGetProperty(out incomingHalDamageModifier) == false)
                {
                    continue;
                }

                instance.BaseDamage += incomingHalDamageModifier.GetBonusDamageRecived(instance);
                instance.DamagePercent += incomingHalDamageModifier.GetBonusDamageRecivedPercent(instance);
                instance.Flags |= incomingHalDamageModifier.GetDamageFlagMask(instance);
            }

            return instance;
        }

        private static void HandleDamageEvent(DamageInstance instance, IEnumerable<StatusApi> defenderEffects, IEnumerable<StatusApi> attackerEffects)
        {
            DamageRecord @event = new(instance);

            //_api.HandleDamageEvent(@event);
            IOutgoingHealDamageHandler attackerHandler;
            IIncomingHealDamageHandler defenderHandler;

            foreach (StatusApi effect in attackerEffects)
            {
                if (effect.TryGetProperty(out attackerHandler) == false)
                {
                    continue;
                }

                attackerHandler.OnDealDamage(@event);
            }

            foreach (StatusApi effect in defenderEffects)
            {
                if (effect.TryGetProperty(out defenderHandler) == false)
                {
                    continue;
                }

                defenderHandler.OnTakeDamage(@event);
            }
        }
    }
}
