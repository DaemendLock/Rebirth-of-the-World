using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services.Skills;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct ActorReviveUseCase
    {
        private readonly IActorRepository _actorRepository;
        private readonly ActorOperations _actorOperations;
        private readonly ICharacterConsciousStateOutput _characterStateOutput;

        public ActorReviveUseCase(IActorRepository stateRepository, ICharacterConsciousStateOutput characterStateOutput, ActorOperations actorOperations)
        {
            _actorRepository = stateRepository;
            _actorOperations = actorOperations;
            _characterStateOutput = characterStateOutput;
        }

        public void Execute(UnitId target, AbilityKey? source)
        {
            if (_actorRepository.TryGet(target, out Actor actor) == false)
            {
                return;
            }

            if (actor.ConsciousState == ConsciousState.Alive)
            {
                return;
            }

            actor.Revive();
            _actorRepository.Update(actor);

            _characterStateOutput.Present(target, actor.ConsciousState);
        }
    }
}
