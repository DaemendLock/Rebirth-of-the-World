using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System.Collections.Generic;

using System.Linq;

namespace Combat.Local.Domain.UseCases
{
    public class UpdateStatusesUseCases
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusTimerRepository _statusTimerRepository;
        private readonly IStatusExpiredEventHandler _statusExpiredEventHandler;
        private readonly IStatusTickEventHandler _statusTickEventHandler;
        private readonly IRemoveStatusEventHandler _removeStatusEventHandler;

        public UpdateStatusesUseCases(IStatusRepository statusRepository, IStatusTimerRepository statusTimerRepository, IStatusExpiredEventHandler statusExpiredEventHandler, IStatusTickEventHandler statusTickEventHandler, IRemoveStatusEventHandler removeStatusEventHandler)
        {
            _statusRepository = statusRepository;
            _statusTimerRepository = statusTimerRepository;
            _statusExpiredEventHandler = statusExpiredEventHandler;
            _statusTickEventHandler = statusTickEventHandler;
            _removeStatusEventHandler = removeStatusEventHandler;
        }

        public void Execute(float deltaTime)
        {
            ICollection<Status> data = _statusRepository.GetAll().ToArray();

            foreach (Status value in data)
            {
                Duration duration = value.Duration;

                if (value.Duration.Left <= 0)
                {
                    _statusRepository.Delete(value.Id);
                    _statusTimerRepository.Delete(value.Id);
                    _removeStatusEventHandler.HandleEvent(value.Id);
                    continue;
                }

                duration.ActiveTime += deltaTime;

                if (duration.Left <= 0)
                {
                    _statusExpiredEventHandler.HandleEvent(value.Id);
                }

                Status status = value;
                status.Duration = duration;
                _statusRepository.Update(status);
            }
        }
    }

    public interface IStatusExpiredEventHandler
    {
        void HandleEvent(StatusId statusId);
    }
}
