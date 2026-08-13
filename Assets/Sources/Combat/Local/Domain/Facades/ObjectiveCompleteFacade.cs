using Combat.Common.ValueObjects;
using Combat.Local.Domain.UseCases.Objectives;

namespace Combat.Local.Domain.Facades
{
    public readonly struct ObjectiveCompleteFacade
    {
        private readonly ObjectiveCompleteUseCase _completeUseCase;
        private readonly ObjectiveCancelUseCase _cancelUseCase;
        private readonly ObjectiveFailUseCase _failUseCase;

        public ObjectiveCompleteFacade(ObjectiveCompleteUseCase completeUseCase, ObjectiveCancelUseCase cancelUseCase, ObjectiveFailUseCase failUseCase)
        {
            _completeUseCase = completeUseCase;
            _cancelUseCase = cancelUseCase;
            _failUseCase = failUseCase;
        }

        public void Complete(ObjectiveId id) => _completeUseCase.Execute(id);

        public void Cancel(ObjectiveId id) => _cancelUseCase.Execute(id);

        public void Fail(ObjectiveId id) => _failUseCase.Execute(id);
    }
}
