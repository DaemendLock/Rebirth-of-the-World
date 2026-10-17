using Combat.Common;
using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Local.Application.OutputPorts;
using Combat.Local.Application.Repositories;

namespace Combat.Local.Application.UseCases
{
    public readonly struct ActorReviveUseCase
    {
        private readonly IActorRepository _actorRepository;
        private readonly ICharacterConsciousStateOutput _characterStateOutput;

        public ActorReviveUseCase(IActorRepository stateRepository, ICharacterConsciousStateOutput characterStateOutput)
        {
            _actorRepository = stateRepository;
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
