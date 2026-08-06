using Combat.Common;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.UseCases;

namespace Combat.Local.Domain.Facades
{
    public readonly struct StatusFacade
    {
        private readonly StatusTimerStartUseCase _startStatusTimerUseCase;
        private readonly StatusTimerStopUseCase _stopStatusTimerUseCase;
        private readonly StatusRemoveUseCase _removeStatusUseCase;

        private readonly IStatusRepository _statusRepository;

        public StatusFacade(StatusTimerStartUseCase startStatusTimerUseCase, StatusTimerStopUseCase stopStatusTimerUseCase, StatusRemoveUseCase removeStatusUseCase, IStatusRepository statusRepository)
        {
            _startStatusTimerUseCase = startStatusTimerUseCase;
            _stopStatusTimerUseCase = stopStatusTimerUseCase;
            _removeStatusUseCase = removeStatusUseCase;
            _statusRepository = statusRepository;
        }

        public void StartPeriodicAction(StatusId target, float period, float startTime = 0)
        {
            _startStatusTimerUseCase.Execute(target, period, startTime);
        }

        public void StopPeriodicAction(StatusId target)
        {
            _stopStatusTimerUseCase.Execute(target);
        }

        public void ExtendDuration(StatusId id, float duration)
        {
            if (_statusRepository.TryGet(id, out Status target) == false)
            {
                return;
            }

            Duration value = target.Duration;
            value.FullDuration += duration;
            _statusRepository.Update(target);
        }

        public Duration GetDuration(StatusId target)
        {
            if (_statusRepository.TryGet(target, out Status data) == false)
            {
                return default;
            }

            return data.Duration;
        }

        public void SetDuration(StatusId id, float duration) => throw new System.NotImplementedException();

        public int GetStackCount(StatusId target)
        {
            if (_statusRepository.TryGet(target, out Status data) == false)
            {
                return default;
            }

            return data.StackCount;
        }

        public void SetStackCount(StatusId id, int stackCount)
        {
            throw new System.NotImplementedException();
        }

        public void Update(StatusId id)
        {

        }
    }
}
