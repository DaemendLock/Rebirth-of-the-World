using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Domain.Repositories;
using Combat.Local.Application.OutputPorts.Statuses;
using Combat.Local.Application.Repositories;
using Combat.Local.Application.ValueObjects;

using System;

namespace Combat.Local.Application.UseCases
{
    public sealed class StatusOwnerProgressAllUseCases
    {
        private readonly IStatusTimerRepository _statusTimerRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;
        private readonly IStatusLifecycleHandler _statusLifecycleHandler;
        private readonly IStatusTickHandler _statusTickHandler;

        public StatusOwnerProgressAllUseCases(IStatusTimerRepository statusTimerRepository, IStatusOwnerRepository statusOwnerRepository, ICharacterUpdateRepository characterUpdateList, IStatusTickHandler statusTickHandler, IStatusLifecycleHandler statusLifecycleHandler)
        {
            _statusTimerRepository = statusTimerRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _characterUpdateList = characterUpdateList;
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
            StatusOwner statusOwner = _statusOwnerRepository.Get(target);
            Span<StatusUpdateResult> results = stackalloc StatusUpdateResult[statusOwner.Count];
            statusOwner.ProgressAll(deltaTime, results);

            foreach (StatusUpdateResult updateResult in results)
            {
                HandleResult(deltaTime, statusOwner, updateResult);
            }
        }

        private void HandleResult(float deltaTime, StatusOwner statusOwner, StatusUpdateResult updateResult)
        {
            if (updateResult.Remove)
            {
                _statusLifecycleHandler.Remove(updateResult.Id);
                _statusTimerRepository.Delete(updateResult.Id);
                statusOwner.Remove(updateResult.Id);
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
                _statusTickHandler.Handle(updateResult.Id);
                timer.TimePassed -= timer.Priod;
            }

            _statusTimerRepository.Update(timer);
        }
    }

    public interface IStatusExpiredEventHandler
    {
        void HandleEvent(StatusId statusId);
    }
}
