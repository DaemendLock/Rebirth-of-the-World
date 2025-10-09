using Combat.API.Statuses;
using Combat.Common.ValueObjects;
using Combat.Local.Controllers;
using Combat.Local.Domain.ValueObjects;

namespace Combat.API
{
    public sealed class StatusApi
    {
        private readonly StatusId _id;
        private readonly Unit _parent;
        private readonly SkillApi _source;

        private readonly StatusController _statusController;
        private readonly StatusScript _script;

        public StatusApi(StatusId id, Unit parent, SkillApi source, SceneApi scene, StatusController statusController, StatusScript script)
        {
            _id = id;
            _statusController = statusController;
            _parent = parent;
            _source = source;
            _script = script;
            script.Init(new(this, parent, source, scene));
        }

        public StatusId Id => _id;

        public Unit Parent => _parent;

        public SkillApi Source => _source;

        public int StackCount { get => _statusController.GetStackCount(_id); set => _statusController.SetStackCount(_id, value); }

        public Duration Duration => _statusController.GetDuration(_id);

        public void StartPeriodicAction(float interval)
        {
            _statusController.StartPeriodicAction(Id, interval);
        }

        public void StopPeriodocAction()
        {
            _statusController.StopPeriodicAction(Id);
        }

        public void ExtendDuration(float duration)
        {
            _statusController.ExtendDuration(_id, duration);
        }

        public bool TryGetProperty<T>(out T property) where T : class, IStatusPropery
        {
            if (_script is not T result)
            {
                property = default;
                return false;
            }

            property = result;
            return true;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override bool Equals(object obj) => obj is StatusApi status && status.Id == Id;
    }
}
