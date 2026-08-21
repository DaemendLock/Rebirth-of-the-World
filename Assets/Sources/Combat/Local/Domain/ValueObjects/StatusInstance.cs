using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.ValueObjects
{
    public readonly struct StatusInstance
    {
        public readonly StatusId StatusId;
        public readonly StatusType Type;
        public readonly Duration Duration;
        public readonly bool IsDead;

        public StatusInstance(StatusId statusId, StatusType type, Duration duration, bool isDead = false)
        {
            StatusId = statusId;
            Type = type;
            Duration = duration;
            IsDead = isDead;
        }

        public StatusInstance MarkDead() => new(StatusId, Type, Duration, true);
    }
}
