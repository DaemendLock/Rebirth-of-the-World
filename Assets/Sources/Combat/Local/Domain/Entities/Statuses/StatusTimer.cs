using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities.Statuses;

namespace Combat.Local.Domain.Entities
{
    public struct StatusTimer
    {
        private readonly IStatusStrategy _statusStrategy;

        public StatusTimer(StatusId statusId, float priod, IStatusStrategy strategy, float timePassed = 0)
        {
            StatusId = statusId;
            TimePassed = timePassed;
            Priod = priod;
            _statusStrategy = strategy;
        }

        public StatusId StatusId { get; }
        public float Priod { get; }
        public float TimePassed { get; set; }

        public readonly void Tick() => _statusStrategy.Tick();
    }
}
