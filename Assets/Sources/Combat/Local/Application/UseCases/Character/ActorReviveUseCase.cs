using Combat.Common;
using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.OutputPorts;

namespace Combat.Local.Application.UseCases
{
    public readonly struct ActorReviveUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly ICharacterConsciousStateOutput _characterStateOutput;

        public ActorReviveUseCase(IUnitStore unitStore, ICharacterConsciousStateOutput characterStateOutput)
        {
            _unitStore = unitStore;
            _characterStateOutput = characterStateOutput;
        }

        public void Execute(UnitId target, AbilityKey? source)
        {
            ref LifeStateOwner lifeState = ref _unitStore.Get(target).LifeState;

            if (lifeState.TryRevive() == false)
            {
                return;
            }

            _characterStateOutput.Present(target, ConsciousState.Alive);
        }
    }
}
