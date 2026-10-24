using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.UseCases;

namespace Combat.Local.Application.Facades
{
    public readonly struct StatusFacade
    {
        private readonly StatusTimerStartUseCase _startTimer;
        private readonly StatusTimerStopUseCase _stopTimer;
        private readonly IUnitStore _unitStore;

        public StatusFacade(StatusTimerStartUseCase startTimer, StatusTimerStopUseCase stopTimer, IUnitStore unitStore)
        {
            _startTimer = startTimer;
            _stopTimer = stopTimer;
            _unitStore = unitStore;
        }

        public void StartPeriodicAction(StatusId id, float period, float startTime = 0) =>
            _startTimer.Execute(id, period, startTime);

        public void StopPeriodicAction(StatusId id) =>
            _stopTimer.Execute(id);

        public Duration GetDuration(UnitId parent, StatusId id) =>
            _unitStore.Get(parent).Statuses.TryGet(id, out StatusInstance instance) ? instance.Duration : default;

        public int GetStackCount(UnitId parent, StatusId id) =>
            _unitStore.Get(parent).Statuses.TryGet(id, out StatusInstance instance) ? instance.StackCount : 0;

        public void ExtendDuration(UnitId parent, StatusId id, float duration) =>
            _unitStore.Get(parent).Statuses.ExtendDuration(id, duration);

        public void SetDuration(UnitId parent, StatusId id, float duration) =>
            _unitStore.Get(parent).Statuses.SetDuration(id, duration);

        public void SetStackCount(UnitId parent, StatusId id, int stackCount) =>
            _unitStore.Get(parent).Statuses.SetStackCount(id, stackCount);
    }
}
