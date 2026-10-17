using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;

namespace Combat.Local.Application.Facades
{
    public readonly struct StatusFacade
    {
        private readonly StatusTimerStartUseCase _startTimer;
        private readonly StatusTimerStopUseCase _stopTimer;
        private readonly IStatusOwnerRepository _statusOwnerRepository;

        public StatusFacade(StatusTimerStartUseCase startTimer, StatusTimerStopUseCase stopTimer, IStatusOwnerRepository statusOwnerRepository)
        {
            _startTimer = startTimer;
            _stopTimer = stopTimer;
            _statusOwnerRepository = statusOwnerRepository;
        }

        public void StartPeriodicAction(StatusId id, float period, float startTime = 0) => _startTimer.Execute(id, period, startTime);

        public void StopPeriodicAction(StatusId id) => _stopTimer.Execute(id);

        public Duration GetDuration(UnitId parent, StatusId id) =>
            _statusOwnerRepository.TryGet(parent, out StatusOwner owner) && owner.TryGet(id, out StatusInstance instance) ? instance.Duration : default;

        public int GetStackCount(UnitId parent, StatusId id) =>
            _statusOwnerRepository.TryGet(parent, out StatusOwner owner) && owner.TryGet(id, out StatusInstance instance) ? instance.StackCount : 0;

        public void ExtendDuration(UnitId parent, StatusId id, float duration)
        {
            if (_statusOwnerRepository.TryGet(parent, out StatusOwner owner))
                owner.ExtendDuration(id, duration);
        }

        public void SetDuration(UnitId parent, StatusId id, float duration)
        {

            if (_statusOwnerRepository.TryGet(parent, out StatusOwner owner))
                owner.SetDuration(id, duration);
        }

        public void SetStackCount(UnitId parent, StatusId id, int stackCount)
        {
            if (_statusOwnerRepository.TryGet(parent, out StatusOwner owner))
            {
                owner.SetStackCount(id, stackCount);
            }
        }
    }
}
