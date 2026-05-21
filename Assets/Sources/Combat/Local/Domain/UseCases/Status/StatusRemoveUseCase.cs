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

        public void Execute(StatusId targetId)
        {
            if (_statusRepository.TryGet(targetId, out var target) == false)
            {
                throw new System.InvalidOperationException();
            }

            target.Properties.Remove();
            _statusRepository.Delete(targetId);
            _statusTimerRepository.Delete(targetId);
            StatusOwner statusOwner = _statusOwnerRepository.Get(target.Parent);
            ReadOnlySpan<StatusId> buffer = statusOwner.GetAll();
            Span<StatusId> newValues = stackalloc StatusId[buffer.Length - 1];

            int i = 0;
            foreach (StatusId value in buffer)
            {
                if (value == targetId)
                {
                    continue;
                }

                newValues[i++] = value;
            }

            _statusOwnerRepository.Update(new(statusOwner.Id, newValues));
        }
    }
}
