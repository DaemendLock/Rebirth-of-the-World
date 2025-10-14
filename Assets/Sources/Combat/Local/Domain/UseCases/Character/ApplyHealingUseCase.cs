using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct ApplyHealingUseCase
    {
        private readonly IHealingDamageInstanceRepository _healingDamageInstanceRepository;
        private readonly IHealthRepository _healthRepository;
        private readonly IHealthOutput _healthOutput;
        private readonly IApplyHealingEventHandler _applyHealingEventHandler;

        public ApplyHealingUseCase(IHealingDamageInstanceRepository healingDamageInstanceRepository, IHealthRepository healthRepository, IHealthOutput healthOutput, IApplyHealingEventHandler applyHealingEventHandler)
        {
            _healingDamageInstanceRepository = healingDamageInstanceRepository;
            _healthRepository = healthRepository;
            _healthOutput = healthOutput;
            _applyHealingEventHandler = applyHealingEventHandler;
        }

        public void Execute(EntityId target, float healing, HealingFlags flags, EntityId? healer, EventSource source)
        {
            HealingInstance value = new(target, healing, healing, flags, healer, source);
            HealingInstance instance = _healingDamageInstanceRepository.GetHealingInstance(value);

            healing = instance.FinalHealing;
            flags = instance.Flags;

            Health health = _healthRepository.Get(target);

            health.CurrentHealth += healing;

            if (health.CurrentHealth > health.MaxHealth)
            {
                health.CurrentHealth = health.MaxHealth;
            }

            _healthRepository.Update(health);

            _healthOutput.Present(health);

            if (instance.Flags.HasFlag(HealingFlags.CanRevive) && health.CurrentHealth > 0)
            {
                //Revive(new(data.Healer, data.Source, ReviveFlags.Healed));
            }

            if (instance.Flags.HasFlag(HealingFlags.NonReactable) == false)
            {
                _applyHealingEventHandler.HandleEvent(instance);
            }
        }
    }

    public interface IApplyHealingEventHandler
    {
        void HandleEvent(HealingInstance instance);
    }
}
