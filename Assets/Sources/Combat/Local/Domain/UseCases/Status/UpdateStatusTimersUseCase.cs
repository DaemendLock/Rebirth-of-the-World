using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;
using System.Linq;

namespace Combat.Local.Domain.UseCases
{
    public class UpdateStatusTimersUseCase
    {
        private readonly IStatusTimerRepository _statusTimeRepository;
        private readonly IStatusTickEventHandler _statusTickEventHandler;

        public UpdateStatusTimersUseCase(IStatusTimerRepository statusTimeRepository, IStatusTickEventHandler statusTickEventHandler)
        {
            _statusTimeRepository = statusTimeRepository;
            _statusTickEventHandler = statusTickEventHandler;
        }

        public void Execute(float deltaTime)
        {
            ICollection<StatusTimer> statusTimers = _statusTimeRepository.GetAll().ToArray();

            foreach (StatusTimer timer in statusTimers)
            {
                StatusTimer value = timer;
                value.TimePassed += deltaTime;

                if (value.TimePassed >= value.Priod)
                {
                    value.TimePassed -= value.Priod;
                    _statusTickEventHandler.HandleEvent(value.StatusId);
                }

                _statusTimeRepository.Update(value);
            }
        }
    }

    public interface IStatusTickEventHandler
    {
        void HandleEvent(StatusId statusId);
    }
}
