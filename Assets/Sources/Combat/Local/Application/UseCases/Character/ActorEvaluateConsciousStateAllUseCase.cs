using Combat.Common;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Local.Application.DTO;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.Repositories;
using Combat.Local.Domain.Repositories;

using System;

namespace Combat.Local.Application.UseCases
{
    public sealed class ActorEvaluateConsciousStateAllUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IActorRepository _actorRepository;
        private readonly ICharacterDeathHandler _characterDeathHandler;
        private readonly IHealthOutput _healthOutput;
        private readonly ICharacterConsciousStateOutput _characterConsciousStateOutput;

        public ActorEvaluateConsciousStateAllUseCase(IUnitStore unitStore, IActorRepository actorRepository, IHealthOutput healthOutput, ICharacterDeathHandler characterDeathHandler, ICharacterConsciousStateOutput characterConsciousStateOutput)
        {
            _unitStore = unitStore;
            _actorRepository = actorRepository;
            _healthOutput = healthOutput;
            _characterDeathHandler = characterDeathHandler;
            _characterConsciousStateOutput = characterConsciousStateOutput;
        }

        public void Execute(ReadOnlySpan<Updatable> target, float deltaTime)
        {
            foreach (Updatable item in target)
            {
                if (_actorRepository.TryGet(item.Id, out Actor actor) == false)
                {
                    continue;
                }

                switch (actor.ConsciousState)
                {
                    case ConsciousState.Alive:
                        UpdateAlive(actor);
                        continue;
                    case ConsciousState.Downed:
                        UpdateDowned(actor, deltaTime * item.TimeScale);
                        continue;
                    case ConsciousState.Dead:
                        continue;
                    default:
                        throw new InvalidOperationException($"Failed to update ConsciousState for {item.Id}: Unknows ConsciousState");
                }
            }
        }

        private void UpdateAlive(Actor actor)
        {
            ref UnitContext unit = ref _unitStore.Get(actor.Id);

            if (unit.Health.Current > 0)
            {
                return;
            }

            float damageSeverity = 1 + (-unit.Health.Current / unit.Health.Max);

            if (unit.Composure.Current > 0 && damageSeverity <= Actor.MaxDamageSeverity)
            {
                actor.Down();
                _actorRepository.Update(actor);
                _characterConsciousStateOutput.Present(actor.Id, actor.ConsciousState);
                return;
            }

            actor.Kill();
            _actorRepository.Update(actor);
            _characterDeathHandler.Handle(new(unit.Id));
            _characterConsciousStateOutput.Present(actor.Id, actor.ConsciousState);
        }

        private void UpdateDowned(Actor actor, float deltaTime)
        {
            ref UnitContext unit = ref _unitStore.Get(actor.Id);

            if (unit.Health.Current > 0)
            {
                actor.Revive();
                _actorRepository.Update(actor);
                _characterConsciousStateOutput.Present(actor.Id, actor.ConsciousState);
                return;
            }

            float damageSeverity = 1 + (-unit.Health.Current / unit.Health.Max);

            if (damageSeverity > Actor.MaxDamageSeverity || unit.Composure.Current <= 0)
            {
                actor.Kill();
                _actorRepository.Update(actor);
                _characterDeathHandler.Handle(new(unit.Id));
                _characterConsciousStateOutput.Present(actor.Id, actor.ConsciousState);
                return;
            }

            unit.Composure.TakeDamage(unit.Composure.Max * ComposureOwner.BaseDrainRate * deltaTime * damageSeverity);
            _healthOutput.Present(new ComposureChangeRecord(unit.Id, unit.Composure.Current, unit.Composure.Max));
        }
    }
}
