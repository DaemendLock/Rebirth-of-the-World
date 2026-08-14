using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;

using System;

namespace Combat.Local.Domain.UseCases
{
    public sealed class StatusOwnerProgressAllUseCases
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusTimerRepository _statusTimerRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;
        private readonly IStatusLifecycleHandler _statusLifecycleHandler;
        private readonly IStatusTickHandler _statusTickHandler;

        public StatusOwnerProgressAllUseCases(IStatusRepository statusRepository, IStatusTimerRepository statusTimerRepository, IStatusOwnerRepository statusOwnerRepository, ICharacterUpdateRepository characterUpdateList, IStatusTickHandler statusTickHandler, IStatusLifecycleHandler statusLifecycleHandler)
        {
            _statusRepository = statusRepository;
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

            ReadOnlySpan<StatusId> ids = statusOwner.GetAll();

            foreach (StatusId statusId in ids)
            {
                if (_statusRepository.TryGet(statusId, out Status value) == false)
                {
                    continue;
                }

                if (TryExpireStatus(statusOwner, value))
                {
                    continue;
                }

                ProgressStatus(deltaTime, value);
            }
        }

        private void ProgressStatus(float deltaTime, Status value)
        {
            value.Progreess(deltaTime);

            if (value.Duration.Left <= 0)
            {
                _statusLifecycleHandler.Expire(value.Id);
            }

            if (_statusTimerRepository.TryGet(value.Id, out StatusTimer timer))
            {
                timer.TimePassed += deltaTime;

                if (timer.TimePassed > timer.Priod)
                {
                    _statusTickHandler.Handle(value.Id);
                    timer.TimePassed -= timer.Priod;
                }

                _statusTimerRepository.Update(timer);
            }

            _statusRepository.Update(value);
        }

        private bool TryExpireStatus(StatusOwner statusOwner, Status status)
        {
            if (status.Duration.Left > 0)
            {
                return false;
            }

            _statusLifecycleHandler.Remove(status.Id);
            _statusRepository.Delete(status.Id);
            _statusTimerRepository.Delete(status.Id);
            //_removeStatusEventHandler.HandleEvent(status.Id);

            var buffer = statusOwner.GetAll();
            Span<StatusId> newValues = stackalloc StatusId[buffer.Length - 1];

            int i = 0;
            foreach (StatusId value in buffer)
            {
                if (value == status.Id)
                {
                    continue;
                }

                newValues[i++] = value;
            }

            _statusOwnerRepository.Update(new(statusOwner.Id, newValues));

            return true;
        }
    }

    public interface IStatusExpiredEventHandler
    {
        void HandleEvent(StatusId statusId);
    }
}
