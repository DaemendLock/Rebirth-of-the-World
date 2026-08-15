using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public sealed class ActorForceKillUseCase
    {
        private readonly IActorRepository _actorRepository;
        private readonly ICharacterConsciousStateOutput _characterStateOutput;

        public ActorForceKillUseCase(IActorRepository stateRepository, ICharacterConsciousStateOutput characterStateOutput)
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

            if (actor.ConsciousState == ConsciousState.Dead)
            {
                return;
            }

            actor.Kill();
            _actorRepository.Update(actor);
            _characterStateOutput.Present(target, actor.ConsciousState);
        }
    }
}
