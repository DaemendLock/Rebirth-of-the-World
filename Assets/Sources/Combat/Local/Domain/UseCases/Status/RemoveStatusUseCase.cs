using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public class RemoveStatusUseCase
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusTimerRepository _statusTimerRepository;

        private readonly IRemoveStatusEventHandler _removeStatusEventHandler;

        public RemoveStatusUseCase(IStatusRepository statusRepository, IStatusTimerRepository statusTimerRepository, IRemoveStatusEventHandler removeStatusEventHandler)
        {
            _statusRepository = statusRepository;
            _statusTimerRepository = statusTimerRepository;
            _removeStatusEventHandler = removeStatusEventHandler;
        }

        public void Execute(StatusId target)
        {
            _statusRepository.Delete(target);
            _statusTimerRepository.Delete(target);
            _removeStatusEventHandler.HandleEvent(target);
        }
    }

    public interface IRemoveStatusEventHandler
    {
        void HandleEvent(StatusId id);
    }
}
