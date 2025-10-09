using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public class StopStatusTimerUseCase
    {
        private readonly IStatusTimerRepository _statusTimeRepository;

        public StopStatusTimerUseCase(IStatusTimerRepository statusTimeRepository)
        {
            _statusTimeRepository = statusTimeRepository;
        }

        public void Execute(StatusId statusId)
        {
            _statusTimeRepository.Delete(statusId);
        }
    }
}
