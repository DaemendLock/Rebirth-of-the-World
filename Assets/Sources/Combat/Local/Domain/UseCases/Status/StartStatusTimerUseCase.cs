using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public class StartStatusTimerUseCase
    {
        private readonly IStatusTimerRepository _statusTimeRepository;

        public StartStatusTimerUseCase(IStatusTimerRepository statusTimeRepository)
        {
            _statusTimeRepository = statusTimeRepository;
        }

        public void Execute(StatusId statusId, float period, float timePassed)
        {
            _statusTimeRepository.Create(new(statusId, period, timePassed));
        }
    }
}
