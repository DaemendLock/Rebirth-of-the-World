using Combat.Common.Primitives;
using Combat.Domain.Repositories;

namespace Combat.Local.Application.UseCases
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
