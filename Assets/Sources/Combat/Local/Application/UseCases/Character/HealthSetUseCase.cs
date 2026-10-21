using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
using Combat.Local.Application.ValueObjects;

namespace Combat.Local.Application.UseCases
{
    public readonly struct HealthSetUseCase
    {
        private readonly IHealthOwnerRepository _healthRepository;
        private readonly IHealthOutput _healthOutput;

        public HealthSetUseCase(IHealthOwnerRepository healthRepository, IHealthOutput healthOutput)
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

        public void SetComposure(UnitId target, float value)
        {
            ref HealthOwner healthOwner = ref _healthRepository.Get(target);
            float oldValue = healthOwner.CurrentComposure;
            healthOwner.CurrentComposure = value;
            _healthOutput.Present(new ComposureChangeRecord(healthOwner.Id, healthOwner.CurrentComposure, healthOwner.MaxComposure, oldValue));
        }
    }

    public interface IHealthOutput
    {
        void Present(HealthChangeRecord record);
        void Present(ComposureChangeRecord record);
        void Present(DamageInstance instance);
    }
}
