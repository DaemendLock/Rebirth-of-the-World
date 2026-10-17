using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
using Combat.Local.Application.ValueObjects;

namespace Combat.Local.Application.UseCases
{
    public readonly struct HealthSetUseCase
    {
        private readonly IHealthRepository _healthRepository;
        private readonly IHealthOutput _healthOutput;

        public HealthSetUseCase(IHealthRepository healthRepository, IHealthOutput healthOutput)
        {
            _healthRepository = healthRepository;
            _healthOutput = healthOutput;
        }

        public void Execute(UnitId target, float value)
        {
            ref HealthOwner healthOwner = ref _healthRepository.Get(target);
            float oldValue = healthOwner.CurrentHealth;
            healthOwner.CurrentHealth = value;
            _healthOutput.Present(new HealthChangeRecord(healthOwner.Id, healthOwner.CurrentHealth, healthOwner.MaxHealth, oldValue));
        }
    }

    public interface IHealthOutput
    {
        void Present(HealthChangeRecord record);
        void Present(DamageInstance instance);
    }
}
