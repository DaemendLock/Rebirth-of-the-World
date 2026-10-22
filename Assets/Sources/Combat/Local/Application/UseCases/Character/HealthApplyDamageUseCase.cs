using Combat.Common;
using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.DTO;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.OutputPorts.Statuses;
using Combat.Local.Application.Queries.Statuses;
using Combat.Local.Application.Repositories;
using Combat.Local.Application.ValueObjects;
using Combat.Local.Domain.Repositories;

using System.Buffers;
using System.Linq;

namespace Combat.Local.Application.UseCases
{
    public readonly struct HealthApplyDamageUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IActorRepository _actorRepository;

        private readonly IHealthOutput _healthOutput;
        private readonly ICharacterConsciousStateOutput _characterConsciousStateOutput;
        private readonly IHealingDamageModificationCalculator _damageModificationCalculator;
        private readonly IDamageResultHandler _damageResultHandler;

        private readonly ICharacterDeathHandler _characterDeathHandler;

        public HealthApplyDamageUseCase(IUnitStore units, IActorRepository stateRepository, IHealthOutput healthOutput, ICharacterConsciousStateOutput characterConsciousStateOutput, IHealingDamageModificationCalculator damageModifierCalculator, IDamageResultHandler damageResultHandler, ICharacterDeathHandler characterDeathHandler)
        {
            _unitStore = units;
            _healthOutput = healthOutput;
            _actorRepository = stateRepository;
            _characterConsciousStateOutput = characterConsciousStateOutput;
            _damageModificationCalculator = damageModifierCalculator;
            _damageResultHandler = damageResultHandler;
            _characterDeathHandler = characterDeathHandler;
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

            ref UnitContext defender = ref _unitStore.Get(info.Target);

            defenderModification = _damageModificationCalculator.GetIncomingDamageModification(defender.Statuses.GetAll().Select(value => value.Id), instance);
            versatilityModifier /= defender.Attributes.GetVersalityModifier();

            if (instance.Attacker.HasValue)
            {
                try
                {
                    ref UnitContext attacker = ref _unitStore.Get(info.Attacker.Value);
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
            ref UnitContext target = ref _unitStore.Get(instance.Target);

            float originalHealth = target.Health.CurrentHealth;
            float originalComposure = target.Composure.CurrentComposure;
            float finalHealthDamage = instance.HealthDamage;
            float finalComposureDamage = System.Math.Min(instance.ComposureDamage, target.Composure.CurrentComposure);

            if (finalHealthDamage >= target.Health.CurrentHealth && instance.Flags.HasFlag(DamageFlags.NonLethal))
            {
                finalHealthDamage = System.Math.Max(0f, target.Health.CurrentHealth - 1f);
            }

            target.Health.TakeDamage(finalHealthDamage);
            target.Composure.TakeDamage(finalComposureDamage);
            instance.HealthDamage = finalHealthDamage;
            instance.ComposureDamage = finalComposureDamage;
            _healthOutput.Present(instance);
            _healthOutput.Present(new HealthChangeRecord(target.Id, target.Health.CurrentHealth, target.Health.MaxHealth, originalHealth));
            _healthOutput.Present(new ComposureChangeRecord(target.Id, target.Composure.CurrentComposure, target.Composure.MaxComposure, originalComposure));

            return new(instance.Target, instance.OriginalHealthDamage, instance.OriginalComposureDamage,
                finalHealthDamage, finalComposureDamage, instance.Flags, instance.Attacker, instance.Source);

        }

        private void UpdateConsciosState(DamageInstance instance)
        {
            ref HealthOwner health = ref _unitStore.Get(instance.Target).Health;

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
            try
            {
                ref UnitContext target = ref _unitStore.Get(@event.Target);
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
            catch { }

            if (@event.Attacker.HasValue)
            {
                try
                {
                    ref UnitContext attacker = ref _unitStore.Get(@event.Attacker.Value);
                    var statuses = attacker.Statuses.GetAll();
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
                catch { }
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
