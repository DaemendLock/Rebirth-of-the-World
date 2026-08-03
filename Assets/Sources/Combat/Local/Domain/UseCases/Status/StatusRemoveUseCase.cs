using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System;

namespace Combat.Local.Domain.UseCases
{
    public class StatusRemoveUseCase
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusTimerRepository _statusTimerRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;

        public StatusRemoveUseCase(IStatusRepository statusRepository, IStatusTimerRepository statusTimerRepository)
        {
            _statusRepository = statusRepository;
            _statusTimerRepository = statusTimerRepository;
        }

        public void Execute(UnitId target, StatusId statusId)
        {
            StatusOwner statusOwner = _statusOwnerRepository.Get(target);

            if (statusOwner.HasStatus(statusId) == false)
            {
                throw new InvalidOperationException();
            }

            if (_statusRepository.TryGet(statusId, out var status) == false)
            {
                throw new System.InvalidOperationException();
            }

            status.Properties.Remove();
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
