using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System;

namespace Combat.Local.Domain.UseCases
{
    public class RemoveStatusUseCase
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusTimerRepository _statusTimerRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;

        private readonly IRemoveStatusEventHandler _removeStatusEventHandler;

        public RemoveStatusUseCase(IStatusRepository statusRepository, IStatusTimerRepository statusTimerRepository, IRemoveStatusEventHandler removeStatusEventHandler)
        {
            _statusRepository = statusRepository;
            _statusTimerRepository = statusTimerRepository;
            _removeStatusEventHandler = removeStatusEventHandler;
        }

        public void Execute(StatusId targetId)
        {
            if (_statusRepository.TryGet(targetId, out var target) == false)
            {
                throw new System.InvalidOperationException();
            }

            target.Remove();
            _statusRepository.Delete(targetId);
            _statusTimerRepository.Delete(targetId);
            StatusOwner statusOwner = _statusOwnerRepository.Get(target.Parent);
            var buffer = statusOwner.GetAll();
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

            _removeStatusEventHandler.HandleEvent(targetId);
        }
    }

    public interface IRemoveStatusEventHandler
    {
        void HandleEvent(StatusId id);
    }
}
