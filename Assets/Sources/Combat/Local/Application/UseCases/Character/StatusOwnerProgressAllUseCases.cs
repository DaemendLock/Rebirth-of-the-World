using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Domain.Repositories;
using Combat.Local.Application.OutputPorts.Statuses;
using Combat.Local.Application.ValueObjects;
using Combat.Local.Domain.Repositories;

using System;

namespace Combat.Local.Application.UseCases
{
    public sealed class StatusOwnerProgressAllUseCases
    {
        private readonly IStatusTimerRepository _statusTimerRepository;
        private readonly IUnitStore _unitStore;
        private readonly IStatusLifecycleHandler _statusLifecycleHandler;
        private readonly IStatusTickHandler _statusTickHandler;

        public StatusOwnerProgressAllUseCases(IStatusTimerRepository statusTimerRepository, IUnitStore units, IStatusTickHandler statusTickHandler, IStatusLifecycleHandler statusLifecycleHandler)
        {
            _statusTimerRepository = statusTimerRepository;
            _unitStore = units;
            _statusLifecycleHandler = statusLifecycleHandler;
            _statusTickHandler = statusTickHandler;
        }

        public void Execute(ReadOnlySpan<Updatable> targets, float deltaTime)
        {
            foreach (Updatable target in targets)
            {
                ProgressTarget(target.Id, deltaTime * target.TimeScale);
            }
        }

        private void ProgressTarget(UnitId target, float deltaTime)
        {
            ref UnitContext unit = ref _unitStore.Get(target);
            StatusOwner statusOwner = unit.Statuses;
            Span<StatusUpdateResult> results = stackalloc StatusUpdateResult[statusOwner.Count];
            statusOwner.ProgressAll(deltaTime, results);

            foreach (StatusUpdateResult updateResult in results)
            {
                HandleResult(deltaTime, unit, updateResult);
            }
        }

        private void HandleResult(float deltaTime, UnitContext unit, StatusUpdateResult updateResult)
        {
            if (updateResult.Remove)
            {
                _statusTimerRepository.Delete(updateResult.Id);
                unit.Statuses.Remove(updateResult.Id);
                _statusLifecycleHandler.Remove(updateResult.Id);
                return;
            }

            if (updateResult.Expired)
            {
                _statusLifecycleHandler.Expire(updateResult.Id);
            }

            if (_statusTimerRepository.TryGet(updateResult.Id, out StatusTimer timer) == false)
            {
                return;
            }

            timer.TimePassed += deltaTime;

            if (timer.TimePassed > timer.Priod)
            {
                timer.TimePassed -= timer.Priod;
                _statusTimerRepository.Update(timer);
                _statusTickHandler.Handle(updateResult.Id);
                return;
            }

            _statusTimerRepository.Update(timer);
        }
    }

    public interface IStatusExpiredEventHandler
    {
        void HandleEvent(StatusId statusId);
    }
}
