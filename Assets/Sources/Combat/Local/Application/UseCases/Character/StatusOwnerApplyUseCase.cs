using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Factories;
using Combat.Local.Application.OutputPorts.Statuses;

namespace Combat.Local.Application.UseCases
{
    public readonly struct StatusOwnerApplyUseCase
    {
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly StatusFactory _statusFactory;
        private readonly IStatusLifecycleHandler _statusLyfecycleHandler;

        public StatusOwnerApplyUseCase(StatusFactory statusFactory, IStatusOwnerRepository statusOwnerRepository, IStatusLifecycleHandler statusInitCleanupPort)
        {
            _statusFactory = statusFactory;
            _statusOwnerRepository = statusOwnerRepository;
            _statusLyfecycleHandler = statusInitCleanupPort;
        }

        public StatusId Execute(ApplyStatusInfo data)
        {
            StatusOwner owner = _statusOwnerRepository.Get(data.Target);

            StatusInstance instance = _statusFactory.Create(data.StatusName, data.Ability)
                .WithDuration(new(0, data.InitialDuration))
                .WithStackCount(data.InitialStackCount);
            owner.Add(instance);
            _statusLyfecycleHandler.Apply(owner.Id, instance);
            return instance.Id;
        }
    }
}
