using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.DTO
{
    public class UnitRepositories
    {
        public readonly IHealthRepository HealthRepository;
        public readonly IKillableRepository KillableRepository;
        public readonly IPositionRepository PositionRepository;
        public readonly IStatusRepository StatusRepository;
        public readonly IActionRepository ActionRepository;
        public readonly ICastableRepository CastableRepository;

        public UnitRepositories(IHealthRepository healthRepository,
            IKillableRepository killableRepository,
            IPositionRepository positionRepository,
            IStatusRepository statusRepository,
            IActionRepository actionRepository,
            ICastableRepository castableRepository)
        {
            HealthRepository = healthRepository;
            KillableRepository = killableRepository;
            PositionRepository = positionRepository;
            StatusRepository = statusRepository;
            ActionRepository = actionRepository;
            CastableRepository = castableRepository;
        }
    }
}
