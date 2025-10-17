using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct ApplyDamageUseCase
    {
        private readonly IHealthRepository _healthRepository;
        private readonly DamageInstanceFactory _damageInstanceFactory;
        private readonly IHealthOutput _healthOutput;
        private readonly IApplyDamageEventHandler _applyDamageEventHandler;
        private readonly IHealingDamageInstanceRepository _damageInstanceRepository;

        public ApplyDamageUseCase(IHealthRepository healthRepository, IHealthOutput healthOutput, IApplyDamageEventHandler applyDamageEventHandler, DamageInstanceFactory damageInstanceFactory, IHealingDamageInstanceRepository damageInstanceRepository)
        {
            _healthRepository = healthRepository;
            _healthOutput = healthOutput;
            _applyDamageEventHandler = applyDamageEventHandler;
            _damageInstanceFactory = damageInstanceFactory;
            _damageInstanceRepository = damageInstanceRepository;
        }

        public void Execute(EntityId targetId, float damage, DamageFlags flags, EntityId? attacker, EventSource source)
        {
            DamageInstance instance = _damageInstanceFactory.Create(targetId, damage, attacker, flags, source);
            instance = _damageInstanceRepository.GetDamageInstance(instance);

            EntityId target = instance.Target;
            float finalDamage = instance.Damage;
            Health health = _healthRepository.Get(target);

            health.CurrentHealth -= finalDamage;

            if (health.CurrentHealth < 0 && instance.Flags.HasFlag(DamageFlags.NonLethal))
            {
                health.CurrentHealth = 1;
            }

            _healthRepository.Update(health);

            _healthOutput.Present(health);

            if (instance.Flags.HasFlag(DamageFlags.NonReactable) == false)
            {
                IApplyDamageEventHandler.DamageResult result = new(instance.Target, damage, finalDamage, instance.Flags, instance.Attacker, instance.Source.Skill, instance.Source.Unit);
                _applyDamageEventHandler.HandleEvent(result);
            }
        }
    }
}
