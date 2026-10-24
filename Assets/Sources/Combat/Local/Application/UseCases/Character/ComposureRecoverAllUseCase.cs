using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Domain.Repositories;

using System;

namespace Combat.Local.Application.UseCases
{
    public sealed class ComposureRecoverAllUseCase
    {
        private readonly IUnitStore _unitStore;
        public ComposureRecoverAllUseCase(IUnitStore unitStore)
        {
            _unitStore = unitStore;
        }

        public void Execute(ReadOnlySpan<Updatable> targets, float progressTime)
        {
            foreach (Updatable target in targets)
            {
                HandleTarget(target, progressTime);
            }
        }

        private void HandleTarget(Updatable target, float progressTime)
        {
            UnitContext unit = _unitStore.Get(target.Id);
            float time = progressTime * target.TimeScale;

            if (unit.Composure.HealingCooldown >= time)
            {
                unit.Composure.Progress(time);
                return;
            }

            if (unit.Composure.HealingCooldown > 0)
            {
                time -= unit.Composure.HealingCooldown;
                unit.Composure.HealingCooldown = 0f;
            }

            if (unit.LifeState.State != Common.ConsciousState.Alive)
            {
                return;
            }

            unit.Composure.TakeHealing(unit.Composure.Max * ComposureOwner.RegenRate * time);
            return;
        }
    }
}
