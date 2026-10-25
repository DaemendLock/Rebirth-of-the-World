using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;

using System;

namespace Combat.Local.Application.UseCases
{
    public sealed class ComposureRecoverAllUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IHealthOutput _healthOutput;
        public ComposureRecoverAllUseCase(IUnitStore unitStore, IHealthOutput healthOutput)
        {
            _unitStore = unitStore;
            _healthOutput = healthOutput;
        }

        public void Execute(ReadOnlySpan<Updatable> targets, float deltaTime)
        {
            foreach (Updatable target in targets)
            {
                HandleTarget(target.Id, deltaTime * target.TimeScale);
            }
        }

        private void HandleTarget(UnitId target, float deltaTime)
        {
            UnitContext unit = _unitStore.Get(target);
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
