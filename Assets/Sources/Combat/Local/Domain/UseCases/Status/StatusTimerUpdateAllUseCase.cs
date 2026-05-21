using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

using System.Collections.Generic;
using System.Linq;

namespace Combat.Local.Domain.UseCases
{
    public class StatusTimerUpdateAllUseCase
    {
        private readonly IStatusTimerRepository _statusTimeRepository;
        private readonly ICharacterUpdateRepository _characterUpdateList;
        private readonly IStatusRepository _statusRepository;

        public StatusTimerUpdateAllUseCase(IStatusTimerRepository statusTimeRepository, ICharacterUpdateRepository characterUpdateList, IStatusRepository statusRepository)
        {
            _statusTimeRepository = statusTimeRepository;
            _characterUpdateList = characterUpdateList;
            _statusRepository = statusRepository;
        }

        public void Execute(float deltaTime)
        {
            IReadOnlyCollection<StatusTimer> statusTimers = _statusTimeRepository.GetAll().ToArray();

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
                }

                _statusTimeRepository.Update(value);
            }
        }
    }
}
