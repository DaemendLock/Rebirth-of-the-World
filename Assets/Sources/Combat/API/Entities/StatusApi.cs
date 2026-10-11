using Combat.API.Contexts;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

namespace Combat.API
{
    public sealed class StatusApi
    {
        private readonly IOldStatusContext _statusContext;

        public StatusApi(IOldStatusContext statusContext, Unit parent, AbilityApi source)
        {
            _statusContext = statusContext;
            Parent = parent;
            Source = source;
        }

        public Duration Duration => _statusContext.Duration;

        public StatusId Id => _statusContext.Id;

        public Unit Parent { get; }

        public AbilityApi Source { get; }

        public int StackCount
        {
            get => _statusContext.StackCount;
            set => _statusContext.StackCount = value;
        }

        public void ExtendDuration(float duration) => _statusContext.ExtendDuration(duration);

        public int GetHashCode() => _statusContext.GetHashCode();

        public void StartPeriodicAction(float interval) => _statusContext.StartPeriodicAction(interval);

        public void StopPeriodocAction() => _statusContext.StopPeriodocAction();
    }
}