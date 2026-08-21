using Combat.Common.Primitives;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;

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

        public StatusId Execute(ApplStatusDTO data)
        {
            if (_statusOwnerRepository.TryGet(data.Target, out StatusOwner target) == false)
            {
                throw new System.InvalidOperationException("No status owner found");
            }

            if (TryReapplyStatus(target, data, out StatusId id))
            {
                return id;
            }

            Status status = _statusFactory.Create(data.StatusName, data.Target, data.InitialDuration, data.InitialStackCount, data.Ability);
            RegisterStatus(target, status);
            return status.Id;
        }

        private bool TryReapplyStatus(StatusOwner target, ApplStatusDTO data, out StatusId result)
        {
            ReadOnlySpan<StatusId> ids = target.GetAll();

            StatusType name = data.StatusName;

            foreach (StatusId id in ids)
            {
                if (_statusRepository.TryGet(id, out Status status) == false)
                {
                    continue;
                }

                if (status.Name != name)
                {
                    continue;
                }

                status.RefreshDuration(data.InitialDuration);
                _statusRepository.Update(status);
                _statusLyfecycleHandler.Reapply(status.Id, status.Duration.FullDuration, data.Ability);
                result = id;
                return true;
            }

            result = default;
            return false;
        }

        private void RegisterStatus(StatusOwner target, Status status)
        {
            var buffer = target.GetAll();
            System.Span<StatusId> values = stackalloc StatusId[buffer.Length + 1];
            buffer.CopyTo(values);
            values[^1] = status.Id;
            _statusOwnerRepository.Update(new(target.Id, values));
            _statusRepository.Create(status);
            _statusLyfecycleHandler.Apply(status);
        }
    }
}
