using Combat.Common;
using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.OutputPorts.Statuses;
using Combat.Local.Application.Queries.Statuses;
using Combat.Local.Application.ValueObjects;

using System.Buffers;
using System.Linq;

namespace Combat.Local.Application.UseCases
{
    public readonly struct UnitApplyDamageUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IHealthOutput _healthOutput;
        private readonly IHealingDamageModificationCalculator _damageModificationCalculator;
        private readonly IDamageResultHandler _damageResultHandler;

        public UnitApplyDamageUseCase(IUnitStore units, IHealthOutput healthOutput,
            IHealingDamageModificationCalculator damageModifierCalculator, IDamageResultHandler damageResultHandler)
        {
            _unitStore = units;
            _healthOutput = healthOutput;
            _damageModificationCalculator = damageModifierCalculator;
            _damageResultHandler = damageResultHandler;
        }

        public void Execute(ApplyDamageInfo info)
        {
            DamageInstance instance = CreateDamageInstance(info);
            DamageRecord finalDamage = ApplyDamageInstance(instance);

            if (finalDamage.Flags.HasFlag(DamageFlags.NonReactable) == false)
            {
                PostDamageEvent(finalDamage);
            }
        }

        private DamageInstance CreateDamageInstance(ApplyDamageInfo info)
        {
            DamageInstance instance = new(info.Target, info.HealthDamage, info.ComposureDamage, info.Flags, info.Attacker, info.Source);
            DamageModification defenderModification = default;
            float versatilityModifier = 1;
            DamageModification attackerModification = default;

            UnitContext defender = _unitStore.Get(info.Target);

            defenderModification = _damageModificationCalculator.GetIncomingDamageModification(defender.Statuses.GetAll().Select(value => value.Id), instance);
            versatilityModifier /= defender.Attributes.GetVersalityModifier();

            if (instance.Attacker.HasValue)
            {
                try
                {
                    UnitContext attacker = _unitStore.Get(info.Attacker.Value);
                    attackerModification = _damageModificationCalculator.GetOutgoingDamageModification(attacker.Statuses.GetAll().Select(value => value.Id), instance);
                    versatilityModifier *= attacker.Attributes.GetVersalityModifier();
                }
                catch
                { }
            }

            DamageModification finalModification = defenderModification + attackerModification;

            instance.HealthDamage = finalModification.ApplyTo(instance.HealthDamage) * versatilityModifier;
            instance.ComposureDamage = finalModification.ApplyTo(instance.ComposureDamage) * versatilityModifier;
            instance.Flags |= finalModification.FlagsModification;
            return instance;
        }

        private DamageRecord ApplyDamageInstance(DamageInstance instance)
        {
            UnitContext target = _unitStore.Get(instance.Target);

            float originalHealth = target.Health.Current;
            float originalComposure = target.Composure.Current;
            float finalHealthDamage = instance.HealthDamage;
            float finalComposureDamage = System.Math.Min(instance.ComposureDamage, target.Composure.Current);

            if (instance.Flags.HasFlag(DamageFlags.NonLethal) && finalHealthDamage >= target.Health.Current)
            {
                finalHealthDamage = System.Math.Max(0f, target.Health.Current - 1f);
            }

            target.Health.TakeDamage(finalHealthDamage);
            target.Composure.TakeDamage(finalComposureDamage);
            instance.HealthDamage = finalHealthDamage;
            instance.ComposureDamage = finalComposureDamage;
            target.Composure.HealingCooldown = ComposureOwner.HealingDelay;
            _healthOutput.Present(instance);
            _healthOutput.Present(new HealthChangeRecord(target.Id, target.Health.Current, target.Health.Max, originalHealth));
            _healthOutput.Present(new ComposureChangeRecord(target.Id, target.Composure.Current, target.Composure.Max, originalComposure));

            return new(instance.Target, instance.OriginalHealthDamage, instance.OriginalComposureDamage,
                finalHealthDamage, finalComposureDamage, instance.Flags, instance.Attacker, instance.Source);

        }

        private void PostDamageEvent(DamageRecord @event)
        {
            if (_unitStore.TryGet(@event.Target, out UnitContext target))
            {
                var statuses = target.Statuses.GetAll();
                StatusId[] ids = ArrayPool<StatusId>.Shared.Rent(statuses.Count);
                int i = 0;

                foreach (var item in statuses)
                {
                    ids[i++] = item.Id;
                }

                try
                {
                    _damageResultHandler.NotifyDamageRecieved(@event, ids.Take(i));
                }
                finally
                {
                    ArrayPool<StatusId>.Shared.Return(ids);
                }
            }

            if (@event.Attacker.HasValue && _unitStore.TryGet(@event.Attacker.Value, out var attacker))
            {
                var attackerStatuses = attacker.Statuses.GetAll();
                StatusId[] ids = ArrayPool<StatusId>.Shared.Rent(attackerStatuses.Count);
                int i = 0;

                foreach (var item in attackerStatuses)
                {
                    ids[i++] = item.Id;
                }

                try
                {
                    _damageResultHandler.NotifyDamageDealth(@event, ids.Take(i));
                }
                finally
                {
                    ArrayPool<StatusId>.Shared.Return(ids);
                }
            }
        }
    }
}
