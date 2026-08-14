using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;

using System;

namespace Combat.Local.Domain.UseCases
{
    public class StatusRemoveUseCase
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusTimerRepository _statusTimerRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IStatusLifecycleHandler _statusLifecycleHandler;

        public StatusRemoveUseCase(IStatusRepository statusRepository, IStatusTimerRepository statusTimerRepository,
            IStatusOwnerRepository statusOwnerRepository, IStatusLifecycleHandler statusLifecycleHandler)
        {
            _statusRepository = statusRepository;
            _statusTimerRepository = statusTimerRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _statusLifecycleHandler = statusLifecycleHandler;
        }

        public void Execute(UnitId target, StatusId statusId)
        {
            StatusOwner statusOwner = _statusOwnerRepository.Get(target);

            if (statusOwner.HasStatus(statusId) == false)
            {
                throw new InvalidOperationException();
            }

            _statusLifecycleHandler.Remove(statusId);
            _statusTimerRepository.Delete(statusId);
            _statusRepository.Delete(statusId);

            ReadOnlySpan<StatusId> buffer = statusOwner.GetAll();
            Span<StatusId> newValues = stackalloc StatusId[buffer.Length - 1];

            int i = 0;
            foreach (StatusId value in buffer)
            {
                if (value == statusId)
                {
                    continue;
                }

                newValues[i++] = value;
            }

            _statusOwnerRepository.Update(new(statusOwner.Id, newValues));
        }
    }
}
