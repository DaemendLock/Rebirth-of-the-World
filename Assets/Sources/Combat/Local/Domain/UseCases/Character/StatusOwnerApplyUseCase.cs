using Combat.Common.Primitives;
using Combat.Local.Domain.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;

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
            ref StatusOwner target = ref _statusOwnerRepository.Get(data.Target);

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
            StatusType name = data.StatusName;

            foreach (StatusId id in target.GetAll())
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
            target.AddStatus(status.Id);
            _statusRepository.Create(status);
            _statusLyfecycleHandler.Apply(status);
        }
    }
}
