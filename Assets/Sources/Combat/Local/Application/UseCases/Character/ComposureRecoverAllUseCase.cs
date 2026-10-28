using Combat.Domain.Entities;
using Combat.Local.Application.DTO;

namespace Combat.Local.Application.UseCases
{
    public sealed class ComposureRecoverAllUseCase
    {
        private readonly IHealthOutput _healthOutput;

        public ComposureRecoverAllUseCase(IHealthOutput healthOutput)
        {
            _healthOutput = healthOutput;
        }

        public void Execute(TickContext tick, float deltaTime)
        {
            foreach (UnitContext target in tick.Updatables)
            {
                HandleTarget(target, deltaTime * target.Attributes.TimeScale);
            }
        }

        private void HandleTarget(UnitContext unit, float deltaTime)
        {
            float recoveryTime = UpdateCooldown(unit, deltaTime);

            switch (unit.LifeState.State)
            {
                case Common.ConsciousState.Downed:
                    HandleDown(unit, deltaTime);
                    return;

                case Common.ConsciousState.Alive:
                    HandleAlive(unit, recoveryTime);
                    return;

                default:
                    return;
            }
        }

        private void HandleAlive(UnitContext unit, float recoveryTime)
        {
            ComposureChangeRecord change;
            unit.Composure.TakeHealing(unit.Composure.Max * ComposureOwner.RegenRate * recoveryTime);
            change = new(unit.Id, unit.Composure.Current, unit.Composure.Max);
            _healthOutput.Present(change);
        }

        private void HandleDown(UnitContext unit, float deltaTime)
        {
            ComposureChangeRecord change;
            float damageSeverity = 1 + -unit.Health.Current / unit.Health.Max;
            unit.Composure.TakeDamage(unit.Composure.Max * ComposureOwner.BaseDrainRate * deltaTime * damageSeverity);
            change = new(unit.Id, unit.Composure.Current, unit.Composure.Max);
            _healthOutput.Present(change);
        }

        private float UpdateCooldown(UnitContext unit, float deltaTime)
        {
            if (unit.Composure.HealingCooldown >= deltaTime)
            {
                unit.Composure.Progress(deltaTime);
                return 0;
            }

            if (unit.Composure.HealingCooldown > 0)
            {
                deltaTime -= unit.Composure.HealingCooldown;
                unit.Composure.HealingCooldown = 0f;
            }

            return deltaTime;
        }
    }
}
