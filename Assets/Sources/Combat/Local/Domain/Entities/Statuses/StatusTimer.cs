using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities
{
    public struct StatusTimer
    {
        public StatusTimer(StatusId statusId, float priod, float timePassed = 0)
        {
            StatusId = statusId;
            TimePassed = timePassed;
            Priod = priod;
        }

        public StatusId StatusId { get; }
        public float Priod { get; }
        public float TimePassed { get; set; }
    }
}
