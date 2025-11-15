using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

namespace Combat.Local.Domain.UseCases
{
    public class ForceKillUseCase
    {
        private readonly IStateRepository _stateRepository;

        public void Execute(EntityId target, EventSource source)
        {
            var state = _stateRepository.Get(target);

            if (state.ConsciousState == Entities.ConsciousState.Dead)
            {
                return;
            }

            state.ConsciousState = Entities.ConsciousState.Dead;
            _stateRepository.Update(state);
            return;
        }
    }
    public interface IReviveUnitUseCase
    {
        void Execute(EntityId target, EventSource source);
    }
}
