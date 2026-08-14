using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public class StatusTimerStartUseCase
    {
        private readonly IStatusRepository _statusRepository;
        private readonly IStatusTimerRepository _statusTimeRepository;

        public StatusTimerStartUseCase(IStatusTimerRepository statusTimeRepository, IStatusRepository statusRepository)
        {
            _statusTimeRepository = statusTimeRepository;
            _statusRepository = statusRepository;
        }

        public void Execute(StatusId statusId, float period, float timePassed)
        {
            if (_statusRepository.TryGet(statusId, out _) == false)
            {
                throw new System.InvalidOperationException();
            }

            StatusTimer statusTimer = new(statusId, period, timePassed);
            _statusTimeRepository.Create(statusTimer);
        }
    }
}
