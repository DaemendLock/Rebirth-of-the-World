using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.OutputPorts.Statuses;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Application.UseCases
{
    public sealed class StatusOwnerRemoveUseCase
    {
        private readonly IUnitStore _unitStore;
        private readonly IStatusLifecycleHandler _statusLifecycleHandler;
        private readonly IStatusTimerRepository _statusTimerRepository;

        public StatusOwnerRemoveUseCase(IUnitStore units, IStatusLifecycleHandler statusLifecycleHandler, IStatusTimerRepository statusTimerRepository)
        {
            _unitStore = units;
            _statusLifecycleHandler = statusLifecycleHandler;
            _statusTimerRepository = statusTimerRepository;
        }

        public void Execute(UnitId target, StatusId statusId)
        {
            try
            {
                ref UnitContext unit = ref _unitStore.Get(target);

                if (unit.Statuses.Remove(statusId) == false)
                {
                    return;
                }

                _statusTimerRepository.Delete(statusId);
                _statusLifecycleHandler.Remove(statusId);
            }
            catch { }
        }
    }
}
