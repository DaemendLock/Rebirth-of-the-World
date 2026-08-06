using Combat.API.Contexts;
using Combat.Common;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;

namespace Combat.Local.Scripting.Contexts
{
    public sealed class StatusContext : IStatusContext
    {
        private readonly StatusId _id;

        private readonly StatusFacade _statusFacade;

        public StatusContext(StatusId id, StatusFacade statusController)
        {
            _id = id;
            _statusFacade = statusController;
        }

        public StatusId Id => _id;

        public int StackCount { get => _statusFacade.GetStackCount(_id); set => _statusFacade.SetStackCount(_id, value); }

        public Duration Duration => _statusFacade.GetDuration(_id);

        public void StartPeriodicAction(float interval)
        {
            _statusFacade.StartPeriodicAction(Id, interval);
        }

        public void StopPeriodocAction()
        {
            _statusFacade.StopPeriodicAction(Id);
        }

        public void ExtendDuration(float duration)
        {
            _statusFacade.ExtendDuration(_id, duration);
        }

        public override int GetHashCode() => Id.GetHashCode();
    }
}
