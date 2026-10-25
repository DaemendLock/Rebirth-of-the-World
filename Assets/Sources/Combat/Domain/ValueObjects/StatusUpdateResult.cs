using Combat.Common.Primitives;

namespace Combat.Local.Application.ValueObjects
{
    public readonly struct StatusUpdateResult
    {
        internal StatusUpdateResult(StatusId id, bool expired, float time)
        {
            Id = id;
            Expired = expired;
            TimePassed = time;
        }

        public StatusId Id { get; }
        public bool Expired { get; }
        public float TimePassed { get; }
    }
}
