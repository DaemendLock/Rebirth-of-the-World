using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Local.Application.DTO;
using Combat.Local.Application.Factories;
using Combat.Local.Application.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Application.UseCases
{
    public readonly struct StatusOwnerApplyUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly StatusFactory _statusFactory;
        private readonly IStatusLifecycleHandler _statusLyfecycleHandler;

        public StatusOwnerApplyUseCase(StatusFactory statusFactory, IUnitStore units, IStatusLifecycleHandler statusInitCleanupPort)
        {
            _statusFactory = statusFactory;
            _unitStore = units;
            _statusLyfecycleHandler = statusInitCleanupPort;
        }

        public StatusId Execute(ApplyStatusInfo data)
        {
            StatusOwner owner = _unitStore.Get(data.Target).Statuses;

            StatusInstance instance = _statusFactory.Create(data.StatusName, data.Ability)
                .WithDuration(new(0, data.InitialDuration))
                .WithStackCount(data.InitialStackCount);
            owner.Add(instance);
            _statusLyfecycleHandler.Apply(owner.Id, instance);
            return instance.Id;
        }
    }
}
