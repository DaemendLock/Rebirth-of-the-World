using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.ValueObjects;

namespace Combat.API
{
    public sealed class StatusApi : IStatusApi
    {
        private readonly StatusId _id;

        private readonly StatusFacade _statusFacade;

        public StatusApi(StatusId id, IUnit parent, IAbilityApi source, StatusFacade statusController)
        {
            _id = id;
            _statusFacade = statusController;
            Parent = parent;
            Source = source;
        }

        public StatusId Id => _id;

        public IUnit Parent { get; }

        public IAbilityApi Source { get; }

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
