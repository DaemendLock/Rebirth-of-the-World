using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public class StatusTimerStartUseCase
    {
        private readonly IStatusTimerRepository _statusTimeRepository;

        public StatusTimerStartUseCase(IStatusTimerRepository statusTimeRepository)
        {
            _statusTimeRepository = statusTimeRepository;
        }

        public void Execute(StatusId statusId, float period, float timePassed)
        {
            StatusTimer statusTimer = new(statusId, period, timePassed);
            _statusTimeRepository.Create(statusTimer);
        }
    }
}
