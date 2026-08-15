using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases.Objectives;

namespace Combat.Local.Domain.Facades
{
    public readonly struct ObjectiveCompleteFacade
    {
        private readonly ObjectiveFinalizeUseCase _completeUseCase;

        public ObjectiveCompleteFacade(ObjectiveFinalizeUseCase completeUseCase)
        {
            _completeUseCase = completeUseCase;
        }

        public void Finalize(ObjectiveId id, ObjectiveState state) => _completeUseCase.Execute(id, state);
    }
}
