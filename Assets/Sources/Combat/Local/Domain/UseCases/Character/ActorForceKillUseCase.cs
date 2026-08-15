using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services.Skills;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public sealed class ActorForceKillUseCase
    {
        private readonly IActorRepository _actorRepository;
        private readonly ActorOperations _actorOperations;
        private readonly ICharacterConsciousStateOutput _characterStateOutput;

        public ActorForceKillUseCase(IActorRepository stateRepository, ICharacterConsciousStateOutput characterStateOutput, ActorOperations actorOperations)
        {
            _actorRepository = stateRepository;
            _characterStateOutput = characterStateOutput;
            _actorOperations = actorOperations;
        }

        public void Execute(UnitId target, AbilityKey? source)
        {
            if (_actorRepository.TryGet(target, out Actor actor) == false)
            {
                return;
            }

            if (_actorOperations.TryKill(actor) == false)
            {
                return;
            }

            _characterStateOutput.Present(target, ConsciousState.Dead);
        }
    }
}
