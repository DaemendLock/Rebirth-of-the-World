using Combat.Common.Primitives;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.UseCases
{
    public sealed class StatusOwnerCleanupUseCase
    {
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusTimerRepository _statusTimerRepository;
        private readonly IStatusLifecycleHandler _lifecycleHandler;

        public StatusOwnerCleanupUseCase(IStatusOwnerRepository statusOwnerRepository, IStatusLifecycleHandler lifecycleHandler, IStatusRepository statusRepository, IStatusTimerRepository statusTimerRepository)
        {
            _statusOwnerRepository = statusOwnerRepository;
            _lifecycleHandler = lifecycleHandler;
            _statusRepository = statusRepository;
            _statusTimerRepository = statusTimerRepository;
        }

        public void Execute()
        {
            var values = _statusOwnerRepository.GetAll();

            for (int i = 0; i < values.Length; i++)
            {
                var value = values[i];

                if (value.NeedCleanup == false)
                {
                    continue;
                }

                Span<StatusInstance> instances = value.GetAll();
                Span<int> newIndexes = stackalloc int[instances.Length];
                int cursorNew = 0;
                int cursorRemove = 0;
                Span<StatusId> removedIds = stackalloc StatusId[instances.Length];

                for (int j = 0; j < instances.Length; j++)
                {
                    StatusInstance instance = instances[j];

                    if (instance.IsDead)
                    {
                        removedIds[cursorRemove++] = instance.StatusId;
                        continue;
                    }

                    newIndexes[cursorNew++] = j;
                }

                if (cursorNew == instances.Length)
                {
                    continue;
                }

                StatusInstance[] newValues = new StatusInstance[cursorNew];

                for (int j = 0; j < cursorNew; j++)
                {
                    newValues[j] = instances[newIndexes[j]];
                }

                values[i] = new(value.Id, newValues, false);

                for (int j = 0; j < cursorRemove; j++)
                {
                    _lifecycleHandler.Remove(removedIds[j]);
                    _statusTimerRepository.Delete(removedIds[j]);
                    _statusRepository.Delete(removedIds[j]);
                }
            }
        }
    }
}
