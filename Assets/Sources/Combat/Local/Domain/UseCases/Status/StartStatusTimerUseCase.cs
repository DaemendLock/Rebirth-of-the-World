using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public class StartStatusTimerUseCase
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusTimerRepository _statusTimeRepository;

        public StartStatusTimerUseCase(IStatusTimerRepository statusTimeRepository, IStatusRepository statusRepository)
        {
            _statusTimeRepository = statusTimeRepository;
            _statusRepository = statusRepository;
        }

        public void Execute(StatusId statusId, float period, float timePassed)
        {
            if (_statusRepository.TryGet(statusId, out Status status) == false)
            {
                throw new System.InvalidOperationException();
            }

            _statusTimeRepository.Create(new(statusId, period, status.Strategy, timePassed));
        }
    }
}
