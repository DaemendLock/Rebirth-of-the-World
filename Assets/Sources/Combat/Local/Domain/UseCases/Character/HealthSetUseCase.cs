using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

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
            Health health = _healthRepository.Get(target);
            health.CurrentHealth = value;
            _healthOutput.Present(health);
        }
    }

    public interface ISetHealthEventHandler
    {
        void HandleEvent(Health value, float oldValue);
    }

    public interface IHealthOutput
    {
        void Present(Health health);
    }
}
