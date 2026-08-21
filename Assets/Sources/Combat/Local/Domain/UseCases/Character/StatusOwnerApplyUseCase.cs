using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.UseCases
{
    public readonly struct StatusOwnerApplyUseCase
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly StatusFactory _statusFactory;
        private readonly IStatusLifecycleHandler _statusLyfecycleHandler;

        public StatusOwnerApplyUseCase(IStatusRepository statusRepository, StatusFactory statusFactory, IStatusOwnerRepository statusOwnerRepository, IStatusLifecycleHandler statusInitCleanupPort)
        {
            _statusRepository = statusRepository;
            _statusFactory = statusFactory;
            _statusOwnerRepository = statusOwnerRepository;
            _statusLyfecycleHandler = statusInitCleanupPort;
        }

        public void Execute(ApplStatusDTO data)
        {
            ref StatusOwner target = ref _statusOwnerRepository.Get(data.Target);

            if (TryReapplyStatus(ref target, data))
            {
                return;
            }

            Status status = _statusFactory.Create(data.StatusName, data.Target, data.InitialDuration, data.InitialStackCount, data.Ability);
            RegisterStatus(ref target, status);
        }

        private bool TryReapplyStatus(ref StatusOwner target, ApplStatusDTO data)
        {
            Span<StatusInstance> instances = target.GetAll();

            StatusType name = data.StatusName;

            for (int i = 0; i < instances.Length; i++)
            {
                StatusInstance instance = instances[i];

                if (instance.Type != name)
                {
                    continue;
                }

                Duration duration = new(0, data.InitialDuration);
                instances[i] = new(instance.StatusId, instance.Type, duration);
                _statusLyfecycleHandler.Reapply(instance.StatusId, duration.FullDuration, data.Ability);
                return true;
            }

            return false;
        }

        private void RegisterStatus(ref StatusOwner target, Status status)
        {
            var buffer = target.GetAll();
            StatusInstance[] values = new StatusInstance[buffer.Length + 1];
            buffer.CopyTo(values);
            values[^1] = new(status.Id, status.Name, status.Duration);
            target = new(target.Id, values);
            _statusRepository.Create(status);
            _statusLyfecycleHandler.Apply(status);
        }
    }
}
