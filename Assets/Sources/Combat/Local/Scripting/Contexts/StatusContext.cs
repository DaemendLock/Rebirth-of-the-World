using Combat.API.Contexts;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Application.Facades;

namespace Combat.Local.Scripting.Contexts
{
    public sealed class StatusContext : IOldStatusContext
    {
        private readonly UnitId _parent;
        private readonly StatusId _id;

        private readonly StatusFacade _statusFacade;

        public StatusContext(UnitId parent, StatusId id, StatusFacade statusController)
        {
            _parent = parent;
            _id = id;
            _statusFacade = statusController;
        }

        public StatusId Id => _id;

        public int StackCount { get => _statusFacade.GetStackCount(_parent, _id); set => _statusFacade.SetStackCount(_parent, _id, value); }

        public Duration Duration => _statusFacade.GetDuration(_parent, _id);

        public void StartPeriodicAction(float interval) => _statusFacade.StartPeriodicAction(Id, interval);

        public void StopPeriodocAction() => _statusFacade.StopPeriodicAction(Id);

        public void ExtendDuration(float duration) => _statusFacade.ExtendDuration(_parent, _id, duration);

        public override int GetHashCode() => Id.GetHashCode();
    }
}
