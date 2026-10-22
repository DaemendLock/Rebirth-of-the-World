using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.Repositories;
using Combat.Local.Domain.Repositories;

using System;

namespace Combat.Local.Application.UseCases
{
    public sealed class ComposureRecoverAllUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IActorRepository _actorRepository;

        public ComposureRecoverAllUseCase(IUnitStore unitStore, IActorRepository actorRepository)
        {
            _unitStore = unitStore;
            _actorRepository = actorRepository;
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
            float time = progressTime * target.TimeScale;

            ref UnitContext unit = ref _unitStore.Get(target.Id);

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

            if ((_actorRepository.TryGet(target.Id, out var actor) ? actor.ConsciousState : Common.ConsciousState.Dead) != Common.ConsciousState.Alive)
            {
                return;
            }

            unit.Composure.TakeHealing(unit.Composure.Max * ComposureOwner.RegenRate * time);
            return;
        }
    }
}
