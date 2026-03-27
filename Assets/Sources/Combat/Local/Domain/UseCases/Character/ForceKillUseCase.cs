using Combat.Common.ValueObjects;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct ForceKillUseCase
    {
        private readonly IStateRepository _stateRepository;
        private readonly ICharacterConsciousStateOutput _characterStateOutput;

        public ForceKillUseCase(IStateRepository stateRepository, ICharacterConsciousStateOutput characterStateOutput)
        {
            _stateRepository = stateRepository;
            _characterStateOutput = characterStateOutput;
        }

        public void Execute(EntityId target, EventSource source)
        {
            var state = _stateRepository.Get(target);

            if (state.ConsciousState == Entities.ConsciousState.Dead)
            {
                return;
            }

            state.ConsciousState = Entities.ConsciousState.Dead;
            _stateRepository.Update(state);
            _characterStateOutput.Present(target, state.ConsciousState);
            return;
        }
    }

    public readonly struct ReviveUseCase
    {
        private readonly IStateRepository _stateRepository;
        private readonly ICharacterConsciousStateOutput _characterStateOutput;

        public ReviveUseCase(IStateRepository stateRepository, ICharacterConsciousStateOutput characterStateOutput)
        {
            _stateRepository = stateRepository;
            _characterStateOutput = characterStateOutput;
        }

        public void Execute(EntityId target, EventSource source)
        {
            var state = _stateRepository.Get(target);

            if (state.ConsciousState == Entities.ConsciousState.Alive)
            {
                return;
            }

            state.ConsciousState = Entities.ConsciousState.Alive;
            _stateRepository.Update(state);
            _characterStateOutput.Present(target, state.ConsciousState);
            return;
        }
    }
}
