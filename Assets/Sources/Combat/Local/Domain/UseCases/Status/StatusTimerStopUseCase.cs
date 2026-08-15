using Combat.Common.Primitives;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases
{
    public class StatusTimerStopUseCase
    {
        private readonly IStatusTimerRepository _statusTimeRepository;

        public StatusTimerStopUseCase(IStatusTimerRepository statusTimeRepository)
        {
            _statusTimeRepository = statusTimeRepository;
        }

        public void Execute(StatusId statusId)
        {
            _statusTimeRepository.Delete(statusId);
        }
    }
}
