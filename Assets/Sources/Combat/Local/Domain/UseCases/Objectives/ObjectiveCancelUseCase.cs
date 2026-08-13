using Combat.Common.ValueObjects;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories.Objectives;

namespace Combat.Local.Domain.UseCases.Objectives
{
    public sealed class ObjectiveCancelUseCase
    {
        private readonly IObjectiveRepository _objectiveRepository;
        private readonly IObjectiveCompleteHandler _handler;

        public ObjectiveCancelUseCase(IObjectiveRepository objectiveRepository, IObjectiveCompleteHandler handler)
        {
            _objectiveRepository = objectiveRepository;
            _handler = handler;
        }

        public void Execute(ObjectiveId id)
        {
            if (_objectiveRepository.TryGet(id, out var value) == false)
            {
                throw new System.InvalidOperationException();
            }

            if (value.TryFail() == false)
            {
                throw new System.InvalidOperationException();
            }

            _objectiveRepository.Update(value);
            _handler.Cancel(id);
        }
    }
}
