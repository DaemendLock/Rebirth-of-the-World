using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct ApplyDamageUseCase
    {
        private readonly IHealthRepository _healthRepository;
        private readonly IHealthOutput _healthOutput;
        private readonly IApplyDamageEventHandler _applyDamageEventHandler;
        private readonly IHealingDamageInstanceRepository _damageInstanceRepository;
        private readonly IStateRepository _stateRepository;

        public ApplyDamageUseCase(IHealthRepository healthRepository, IHealthOutput healthOutput, IApplyDamageEventHandler applyDamageEventHandler, IHealingDamageInstanceRepository damageInstanceRepository, IStateRepository stateRepository)
        {
            _healthRepository = healthRepository;
            _healthOutput = healthOutput;
            _applyDamageEventHandler = applyDamageEventHandler;
            _damageInstanceRepository = damageInstanceRepository;
            _stateRepository = stateRepository;
        }

        public void Execute(EntityId targetId, float damage, DamageFlags flags, EntityId? attacker, EventSource source)
        {
            DamageInstance instance = new(targetId, damage, attacker, flags, source);
            instance = _damageInstanceRepository.GetDamageInstance(instance);

            Health health = _healthRepository.Get(instance.Target);
            float finalDamage = instance.Damage;

            if (finalDamage >= health.CurrentHealth && instance.Flags.HasFlag(DamageFlags.NonLethal))
            {
                finalDamage = health.CurrentHealth - 1;
            }

            health.TakeDamage(finalDamage);

            _healthRepository.Update(health);
            _healthOutput.Present(health);

            if (instance.Flags.HasFlag(DamageFlags.NonReactable) == false)
            {
                IApplyDamageEventHandler.DamageResult result = new(instance.Target, damage, finalDamage, instance.Flags, instance.Attacker, instance.Source.Skill, instance.Source.Unit);
                _applyDamageEventHandler.HandleEvent(result);
            }

            if (health.CurrentHealth > 0)
            {
                return;
            }

            Kill(health.Id, attacker, source);
        }

        private void Kill(EntityId target, EntityId? attacker, EventSource source)
        {
            var state = _stateRepository.Get(target);

            if (state.ConsciousState == ConsciousState.Dead)
            {
                return;
            }

            state.ConsciousState = ConsciousState.Dead;
            _stateRepository.Update(state);
            return;
        }
    }
}
