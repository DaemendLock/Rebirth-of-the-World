using Combat.Common.ValueObjects;
using Combat.Local.Domain.ValueObjects;

namespace Combat.API
{
    public interface IStatusApi
    {
        Duration Duration { get; }
        StatusId Id { get; }
        IUnit Parent { get; }
        IAbilityApi Source { get; }
        int StackCount { get; set; }

        void ExtendDuration(float duration);
        int GetHashCode();
        void StartPeriodicAction(float interval);
        void StopPeriodocAction();
    }
}