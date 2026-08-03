using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.UseCases
{
    public class StatusOwnerProgressAllUseCases
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusTimerRepository _statusTimerRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;

        public StatusOwnerProgressAllUseCases(IStatusRepository statusRepository, IStatusTimerRepository statusTimerRepository, IStatusOwnerRepository statusOwnerRepository, ICharacterUpdateRepository characterUpdateList)
        {
            _statusRepository = statusRepository;
            _statusTimerRepository = statusTimerRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _characterUpdateList = characterUpdateList;
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
                value.Properties.Expire();
            }

            if (_statusTimerRepository.TryGet(value.Id, out StatusTimer timer))
            {
                timer.TimePassed += deltaTime;

                if (timer.TimePassed > timer.Priod)
                {
                    timer.Tick();
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

            status.Properties.Remove();
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
