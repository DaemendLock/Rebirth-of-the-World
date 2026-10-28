using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.OutputPorts.Statuses;
using Combat.Local.Application.ValueObjects;

using System;

namespace Combat.Local.Application.UseCases
{
    public sealed class StatusOwnerProgressAllUseCases
    {
        private readonly IStatusTimerRepository _statusTimerRepository;
        private readonly IStatusLifecycleHandler _statusLifecycleHandler;
        private readonly IStatusTickHandler _statusTickHandler;

        public StatusOwnerProgressAllUseCases(IStatusTimerRepository statusTimerRepository, IStatusTickHandler statusTickHandler, IStatusLifecycleHandler statusLifecycleHandler)
        {
            _statusTimerRepository = statusTimerRepository;
            _statusLifecycleHandler = statusLifecycleHandler;
            _statusTickHandler = statusTickHandler;
        }

        public void Execute(ReadOnlySpan<UnitContext> targets, float deltaTime)
        {
            foreach (UnitContext target in targets)
            {
                ProgressTarget(target, deltaTime * target.Attributes.TimeScale);
            }
        }

        private void ProgressTarget(UnitContext unit, float deltaTime)
        {
            StatusOwner statusOwner = unit.Statuses;
            Span<StatusUpdateResult> results = stackalloc StatusUpdateResult[statusOwner.Count];
            statusOwner.ProgressAll(deltaTime, results);

            foreach (StatusUpdateResult updateResult in results)
            {
                HandleResult(unit, updateResult);
            }
        }

        private void HandleResult(UnitContext unit, StatusUpdateResult updateResult)
        {
            AdvanceTimer(updateResult.Id, updateResult.TimePassed);

            if (updateResult.Expired)
            {
                _statusLifecycleHandler.Expire(updateResult.Id);
                _statusTimerRepository.Delete(updateResult.Id);
                unit.Statuses.Remove(updateResult.Id);
                _statusLifecycleHandler.Remove(updateResult.Id);
            }
        }

        private void AdvanceTimer(StatusId status, float value)
        {
            if (_statusTimerRepository.TryGet(status, out StatusTimer timer))
            {
                timer.TimePassed += value;

                if (timer.TimePassed > timer.Priod)
                {
                    timer.TimePassed -= timer.Priod;
                    _statusTimerRepository.Update(timer);
                    _statusTickHandler.Handle(status);
                    return;
                }

                _statusTimerRepository.Update(timer);
            }
        }
    }

    public interface IStatusExpiredEventHandler
    {
        void HandleEvent(StatusId statusId);
    }
}
