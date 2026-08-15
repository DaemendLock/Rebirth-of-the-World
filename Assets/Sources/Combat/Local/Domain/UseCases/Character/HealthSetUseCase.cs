using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
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
            if (_healthRepository.TryGet(target, out Health health) == false)
            {
                throw new System.InvalidOperationException("Not found");
            }

            health.CurrentValue = value;
            _healthRepository.Update(health);
            _healthOutput.Present(health);
        }
    }

    public interface IHealthOutput
    {
        void Present(Health health);
        void Present(DamageInstance instance);
    }
}
