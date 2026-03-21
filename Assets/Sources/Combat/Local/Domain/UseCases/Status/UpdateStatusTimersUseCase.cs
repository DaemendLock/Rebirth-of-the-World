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
        private readonly ICharacterUpdateList _characterUpdateList;
        private readonly IStatusRepository _statusRepository;

        public UpdateStatusTimersUseCase(IStatusTimerRepository statusTimeRepository, IStatusTickEventHandler statusTickEventHandler, ICharacterUpdateList characterUpdateList, IStatusRepository statusRepository)
        {
            _statusTimeRepository = statusTimeRepository;
            _statusTickEventHandler = statusTickEventHandler;
            _characterUpdateList = characterUpdateList;
            _statusRepository = statusRepository;
        }

        public void Execute(float deltaTime)
        {
            ICollection<StatusTimer> statusTimers = _statusTimeRepository.GetAll().ToArray();

            foreach (StatusTimer timer in statusTimers)
            {
                StatusTimer value = timer;

                if (_statusRepository.TryGet(timer.StatusId, out Status status) == false)
                {
                    continue;
                }

                float timeScale = _characterUpdateList.Get(status.Parent).TimeScale;

                value.TimePassed += deltaTime * timeScale;

                if (value.TimePassed >= value.Priod)
                {
                    value.TimePassed -= value.Priod;
                    value.Tick();
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
