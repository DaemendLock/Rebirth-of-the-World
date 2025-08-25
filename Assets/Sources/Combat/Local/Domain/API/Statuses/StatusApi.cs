using Combat.Common.ValueObjects;
using Combat.Local.Domain.API.DTO;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using JetBrains.Annotations;

namespace Combat.Local.Domain.API
{
    public interface IStatusPropery { }

    public interface IStatusPropertyCollection
    {
        bool TryGetProperty<T>(StatusId status, out T property) where T : class, IStatusPropery;
    }

    public class StatusApi
    {
        private IStatusRepository _statusRepository;

        private float _timerDelay;
        private float _timerNextTick;

        public StatusId Id { get; private set; }

        public Unit Parent { get; private set; }

        public ScriptedSkill Source { get; private set; }

        public StatusName Name { get; private set; }

        public int StackCount { get; set; }

        public Duration Duration { get; private set; }

        public bool Update()
        {
            if (_statusRepository.TryGet(Id, out Status data) == false)
            {
                return false;
            }

            //_model = _modelRepository.Invoke(Id);
            StackCount = data.StackCount;
            Duration = data.Duration;

            if (_timerNextTick <= Duration.ActiveTime)
            {
                OnTick();
                _timerNextTick += _timerDelay;
            }

            return true;
        }

        public virtual void OnCreate() { }

        public virtual void OnExpire() { }

        public virtual void OnRemove() { }

        public virtual void OnTick() { }

        public void Remove() => _statusRepository.Delete(Id);

        public void StartPeriodicAction(float interval)
        {
            if (interval <= 0)
            {
                return;
            }

            _timerDelay = interval;
            _timerNextTick = Duration.ActiveTime + interval;
        }

        public void StopPeriodocAction()
        {
            _timerNextTick = float.PositiveInfinity;
        }

        public bool TryGetProperty<T>(out T property) where T : class, IStatusPropery
        {
            if (this is not T result)
            {
                property = default;
                return false;
            }

            property = result;
            return true;
        }

        public override int GetHashCode() => Id.GetHashCode();

        public override bool Equals(object obj) => obj is StatusApi status && status.Id == Id;

        [UsedImplicitly()]
        private void Init(ScriptedStatusContext context, IStatusRepository statusRepository)
        {
            _statusRepository = statusRepository;

            Id = context.Id;
            Parent = context.Parent;
            _timerNextTick = float.PositiveInfinity;
            Source = context.Source;
            Name = context.Name;

            StackCount = context.StackCount;
            Duration = context.Duration;
        }
    }
}
