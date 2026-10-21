using Combat.Common;
using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.OutputPorts.Statuses;
using Combat.Local.Application.Queries.Statuses;
using Combat.Local.Application.Repositories;
using Combat.Local.Application.ValueObjects;

using System.Buffers;
using System.Linq;

namespace Combat.Local.Application.UseCases
{
    public readonly struct HealthApplyDamageUseCase
    {
        private readonly IHealthOwnerRepository _healthRepository;
        private readonly IActorRepository _actorRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IAttributesRepository _attributesRepository;

        private readonly IHealthOutput _healthOutput;
        private readonly ICharacterConsciousStateOutput _characterConsciousStateOutput;
        private readonly IHealingDamageModificationCalculator _damageModificationCalculator;
        private readonly IDamageResultHandler _damageResultHandler;

        private readonly ICharacterDeathHandler _characterDeathHandler;

        public HealthApplyDamageUseCase(IHealthOwnerRepository healthRepository, IHealthOutput healthOutput, IActorRepository stateRepository, IStatusOwnerRepository statusOwnerRepository, ICharacterConsciousStateOutput characterConsciousStateOutput, IHealingDamageModificationCalculator damageModifierCalculator, IDamageResultHandler damageResultHandler, ICharacterDeathHandler characterDeathHandler, IAttributesRepository attributesRepository)
        {
            _healthRepository = healthRepository;
            _healthOutput = healthOutput;
            _actorRepository = stateRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _characterConsciousStateOutput = characterConsciousStateOutput;
            _damageModificationCalculator = damageModifierCalculator;
            _damageResultHandler = damageResultHandler;
            _characterDeathHandler = characterDeathHandler;
            _attributesRepository = attributesRepository;
        }

        public void Execute(ApplyDamageInfo info)
        {
            DamageInstance instance = CreateDamageInstance(info);
            DamageRecord finalDamage = ApplyDamageInstance(instance);

            if (finalDamage.Flags.HasFlag(DamageFlags.NonReactable) == false)
            {
                PostDamageEvent(finalDamage);
            }

            UpdateConsciosState(instance);
        }

        private DamageInstance CreateDamageInstance(ApplyDamageInfo info)
        {
            DamageInstance instance = new(info.Target, info.HealthDamage, info.ComposureDamage, info.Flags, info.Attacker, info.Source);
            DamageModification defenderModification = default;
            float versatilityModifier = 1;
            DamageModification attackerModification = default;

            if (_statusOwnerRepository.TryGet(info.Target, out StatusOwner defenderStatuses))
            {
                AttributesOwner defenderAttributes = _attributesRepository.Get(info.Target);
                defenderModification = _damageModificationCalculator.GetIncomingDamageModification(defenderStatuses.GetAll().Select(value => value.Id), instance);
                versatilityModifier /= defenderAttributes.GetVersalityModifier();
            }

            if (instance.Attacker.HasValue && _statusOwnerRepository.TryGet(instance.Attacker.Value, out StatusOwner attackerStatuses))
            {
                AttributesOwner attackerAttributes = _attributesRepository.Get(info.Target);
                attackerModification = _damageModificationCalculator.GetOutgoingDamageModification(attackerStatuses.GetAll().Select(value => value.Id), instance);
                versatilityModifier *= attackerAttributes.GetVersalityModifier();
            }

            DamageModification finalModification = defenderModification + attackerModification;

            instance.HealthDamage = finalModification.ApplyTo(instance.HealthDamage) * versatilityModifier;
            instance.ComposureDamage = finalModification.ApplyTo(instance.ComposureDamage) * versatilityModifier;
            instance.Flags |= finalModification.FlagsModification;
            return instance;
        }

        private DamageRecord ApplyDamageInstance(DamageInstance instance)
        {
            ref HealthOwner health = ref _healthRepository.Get(instance.Target);

            float originalHealth = health.CurrentHealth;
            float originalComposure = health.CurrentComposure;
            float finalHealthDamage = instance.HealthDamage;
            float finalComposureDamage = System.Math.Min(instance.ComposureDamage, health.CurrentComposure);

            if (finalHealthDamage >= health.CurrentHealth && instance.Flags.HasFlag(DamageFlags.NonLethal))
            {
                finalHealthDamage = System.Math.Max(0f, health.CurrentHealth - 1f);
            }

            health.TakeDamage(finalHealthDamage, finalComposureDamage);
            instance.HealthDamage = finalHealthDamage;
            instance.ComposureDamage = finalComposureDamage;
            _healthOutput.Present(instance);
            _healthOutput.Present(new HealthChangeRecord(health.Id, health.CurrentHealth, health.MaxHealth, originalHealth));
            _healthOutput.Present(new ComposureChangeRecord(health.Id, health.CurrentComposure, health.MaxComposure, originalComposure));

            return new(instance.Target, instance.OriginalHealthDamage, instance.OriginalComposureDamage,
                finalHealthDamage, finalComposureDamage, instance.Flags, instance.Attacker, instance.Source);

        }

        private void UpdateConsciosState(DamageInstance instance)
        {
            ref HealthOwner health = ref _healthRepository.Get(instance.Target);

            if (health.CurrentHealth > 0)
            {
                return;
            }

            //if (instance.Flags.HasFlag(DamageFlags.InstantKill))
            //{
            Kill(health.Id, instance.Attacker, instance.Source);
            //}
            //else
            //{
            //    KnockDown(health.Id, instance.Attacker, instance.Source);
            //}
        }

        private void PostDamageEvent(DamageRecord @event)
        {
            if (_statusOwnerRepository.TryGet(@event.Target, out StatusOwner statusOwner))
            {
                var statuses = statusOwner.GetAll();
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

            if (@event.Attacker.HasValue && _statusOwnerRepository.TryGet(@event.Attacker.Value, out StatusOwner attackerStatuses))
            {
                var statuses = attackerStatuses.GetAll();
                StatusId[] ids = ArrayPool<StatusId>.Shared.Rent(statuses.Count);
                int i = 0;

                foreach (var item in statuses)
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

        private void Kill(UnitId target, UnitId? attacker, AbilityKey? source)
        {
            if (_actorRepository.TryGet(target, out Actor actor) == false)
            {
                return;
            }

            if (actor.ConsciousState == ConsciousState.Dead)
            {
                return;
            }

            actor.Kill();
            _actorRepository.Update(actor);
            _characterDeathHandler.Handle(new(target));
            _characterConsciousStateOutput.Present(target, actor.ConsciousState);
            return;
        }
    }
}
