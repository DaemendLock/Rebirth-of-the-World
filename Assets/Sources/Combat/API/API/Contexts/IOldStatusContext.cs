using Combat.Common;
using Combat.Common.ValueObjects;

namespace Combat.API.Contexts
{
    public interface IOldStatusContext
    {
        Duration Duration { get; }
        StatusId Id { get; }
        int StackCount { get; set; }

        void ExtendDuration(float duration);
        int GetHashCode();
        void StartPeriodicAction(float interval);
        void StopPeriodocAction();
    }
}
