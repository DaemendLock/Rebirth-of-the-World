using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.UseCases
{
    public sealed class StatusOwnerRemoveUseCase
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusTimerRepository _statusTimerRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IStatusLifecycleHandler _statusLifecycleHandler;

        public StatusOwnerRemoveUseCase(IStatusRepository statusRepository, IStatusTimerRepository statusTimerRepository,
            IStatusOwnerRepository statusOwnerRepository, IStatusLifecycleHandler statusLifecycleHandler)
        {
            _statusRepository = statusRepository;
            _statusTimerRepository = statusTimerRepository;
            _statusOwnerRepository = statusOwnerRepository;
            _statusLifecycleHandler = statusLifecycleHandler;
        }

        public void Execute(UnitId target, StatusId statusId)
        {
            ref StatusOwner statusOwner = ref _statusOwnerRepository.Get(target);
            Span<StatusInstance> values = statusOwner.GetAll();
            bool success = false;

            for (int i = 0; i < values.Length; i++)
            {
                if (values[i].StatusId != statusId)
                {
                    continue;
                }

                values[i] = values[i].MarkDead();
                success = true;
                break;
            }

            if (success == false)
            {
                return;
            }

            _statusLifecycleHandler.Remove(statusId);
            _statusTimerRepository.Delete(statusId);
            _statusRepository.Delete(statusId);
            statusOwner = statusOwner.MarkDirty();
        }
    }
}
