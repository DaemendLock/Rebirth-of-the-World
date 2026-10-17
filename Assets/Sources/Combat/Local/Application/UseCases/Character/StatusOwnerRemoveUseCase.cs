using Combat.Common.Primitives;
using Combat.Domain.Entities;
using Combat.Domain.Repositories;
using Combat.Local.Application.OutputPorts.Statuses;

namespace Combat.Local.Application.UseCases
{
    public sealed class StatusOwnerRemoveUseCase
    {
        private readonly IStatusOwnerRepository _statusOwnerRepository;
        private readonly IStatusLifecycleHandler _statusLifecycleHandler;
        private readonly IStatusTimerRepository _statusTimerRepository;

        public StatusOwnerRemoveUseCase(IStatusOwnerRepository statusOwnerRepository, IStatusLifecycleHandler statusLifecycleHandler, IStatusTimerRepository statusTimerRepository)
        {
            _statusOwnerRepository = statusOwnerRepository;
            _statusLifecycleHandler = statusLifecycleHandler;
            _statusTimerRepository = statusTimerRepository;
        }

        public void Execute(UnitId target, StatusId statusId)
        {
            if (_statusOwnerRepository.TryGet(target, out StatusOwner owner) == false)
            {
                return;
            }

            if (owner.Remove(statusId) == false)
            {
                return;
            }

            _statusTimerRepository.Delete(statusId);
            _statusLifecycleHandler.Remove(statusId);
        }
    }
}
