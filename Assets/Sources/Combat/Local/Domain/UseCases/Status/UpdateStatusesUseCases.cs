using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;
using System.Collections.Generic;

namespace Combat.Local.Domain.UseCases
{
    public class UpdateStatusesUseCases
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusTimerRepository _statusTimerRepository;
        private readonly IStatusExpiredEventHandler _statusExpiredEventHandler;
        private readonly IRemoveStatusEventHandler _removeStatusEventHandler;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly ICharacterUpdateList _characterUpdateList;

        public UpdateStatusesUseCases(IStatusRepository statusRepository, IStatusTimerRepository statusTimerRepository, IStatusExpiredEventHandler statusExpiredEventHandler, IRemoveStatusEventHandler removeStatusEventHandler, IStatusOwnerRepository statusOwnerRepository, ICharacterUpdateList characterUpdateList)
        {
            _statusRepository = statusRepository;
            _statusTimerRepository = statusTimerRepository;
            _statusExpiredEventHandler = statusExpiredEventHandler;
            _removeStatusEventHandler = removeStatusEventHandler;
            _statusOwnerRepository = statusOwnerRepository;
            _characterUpdateList = characterUpdateList;
        }

        public void Execute(float deltaTime, IReadOnlyCollection<Updatable> targets)
        {
            foreach (Updatable target in targets)
            {
                UpdateForCharacter(target, deltaTime);
            }
        }

        private void UpdateForCharacter(Updatable target, float deltaTime)
        {
            StatusOwner statusOwner = _statusOwnerRepository.Get(target.Id);

            var ids = statusOwner.GetAll();

            foreach (StatusId statusId in ids)
            {
                if (_statusRepository.TryGet(statusId, out Status value) == false)
                {
                    continue;
                }

                if (TryExpireStatus(value))
                {
                    UnityEngine.Debug.Log($"Status expire: {value.Name}[{value.Id}]");
                    continue;
                }

                Duration duration = value.Duration;
                duration.ActiveTime += deltaTime * target.TimeScale;

                if (duration.Left <= 0)
                {
                    _statusExpiredEventHandler.HandleEvent(value.Id);
                }

                Status status = value;
                status.Duration = duration;
                _statusRepository.Update(status);
            }
        }

        private bool TryExpireStatus(Status status)
        {
            if (status.Duration.Left > 0)
            {
                return false;
            }

            _statusRepository.Delete(status.Id);
            _statusTimerRepository.Delete(status.Id);
            _removeStatusEventHandler.HandleEvent(status.Id);

            StatusOwner statusOwner = _statusOwnerRepository.Get(status.Parent);
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
