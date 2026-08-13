using Combat.Common.ValueObjects;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories.Objectives;

namespace Combat.Local.Domain.UseCases.Objectives
{
    public sealed class ObjectiveCompleteUseCase
    {
        private readonly IObjectiveRepository _objectiveRepository;
        private readonly IObjectiveCompleteHandler _handler;

        public ObjectiveCompleteUseCase(IObjectiveRepository objectiveRepository, IObjectiveCompleteHandler handler)
        {
            _objectiveRepository = objectiveRepository;
            _handler = handler;
        }

        public void Execute(ObjectiveId id)
        {
            if (_objectiveRepository.TryGet(id, out var value) == false)
            {
                throw new System.InvalidOperationException("Objective is not registered");
            }

            if (value.TryComplete() == false)
            {
                throw new System.InvalidOperationException("Unable to complete objective.");
            }

            _objectiveRepository.Update(value);
            _handler.Complete(id);
        }
    }
}
