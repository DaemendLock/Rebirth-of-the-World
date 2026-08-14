using Combat.Common.ValueObjects;
using Combat.Local.Domain.OutputPorts;
using Combat.Local.Domain.Repositories.Objectives;

namespace Combat.Local.Domain.UseCases.Objectives
{
    public sealed class ObjectiveFinalizeUseCase
    {
        private readonly IObjectiveRepository _objectiveRepository;
        private readonly IObjectiveFinalizeHandler _handler;

        public ObjectiveFinalizeUseCase(IObjectiveRepository objectiveRepository, IObjectiveFinalizeHandler handler)
        {
            _objectiveRepository = objectiveRepository;
            _handler = handler;
        }

        public void Execute(ObjectiveId id, ObjectiveState state)
        {
            if (_objectiveRepository.TryGet(id, out var value) == false)
            {
                throw new System.InvalidOperationException("Objective is not registered");
            }

            if (value.TryTransition(state) == false)
            {
                throw new System.InvalidOperationException("Unable to complete objective.");
            }

            _objectiveRepository.Update(value);
            _handler.Finilize(id, value.State);
        }
    }
}
