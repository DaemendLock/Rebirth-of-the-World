using System.Reflection;

using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Implementations.Statuses.Attributes;
using Server.Combat.Domain.Skills;

namespace Server.Combat.Domain.Implementations.Statuses
{
    //[StatusName("Test status"), AuraId(0)]
    //public class Status : IStatusEffect
    //{
    //    private readonly struct PeriodicTime
    //    {
    //        public readonly bool IsActive;
    //        public readonly float TimeInterval;
    //        public readonly float TimeLeft;

    //        public PeriodicTime(bool isActive, float timeInterval) : this(isActive, timeInterval, timeInterval)
    //        { }

    //        private PeriodicTime(bool isActive, float timeInterval, float timeLeft)
    //        {
    //            IsActive = isActive;
    //            TimeInterval = timeInterval;
    //            TimeLeft = timeLeft;
    //        }

    //        public PeriodicTime WithTime(float timeLeft) => new(IsActive, TimeInterval, timeLeft);
    //    }

    //    private PeriodicTime _periodicTime;
    //    private float _activeTime;

    //    public Status(IStatusEffect.StatusContext context)
    //    {
    //        Id = context.StatusId;
    //        AuraId = GetType().GetCustomAttribute<AuraIdAttribute>().Id;
    //        Caster = context.Caster;
    //        Parent = context.Parent;
    //        StackCount = context.StackCount;
    //        Skill = context.Source;
    //        Duration = context.Duration;

    //        _activeTime = 0;
    //        _periodicTime = new(false, 0);
    //    }

    //    public int Id { get;}
    //    public int AuraId { get; }

    //    public Unit Caster { get; }
    //    public Unit Parent { get; }

    //    public ISkill Skill { get; }

    //    public int StackCount { get; set; }

    //    public float ActiveTime
    //    {
    //        get => _activeTime; set
    //        {
    //            float deltaTime = value - _activeTime;
    //            _activeTime = value;

    //            Update(deltaTime);
    //        }
    //    }

    //    public float Duration { get; set; }

    //    public void Destroy() => Parent.RemoveStatus(this);

    //    public void ForceRefresh() => throw new System.NotImplementedException();

    //    protected void StartPeriodicAction(float delay)
    //    {
    //        _periodicTime = new(true, delay);
    //    }

    //    protected void StopPeriodicAction()
    //    {
    //        _periodicTime = new(false, _periodicTime.TimeInterval);
    //    }

    //    protected virtual void PerformPeriodicAction()
    //    { }

    //    private void Update(float deltaTime)
    //    {
    //        float newTime = _periodicTime.TimeLeft - deltaTime;

    //        if (newTime <= 0)
    //        {
    //            PerformPeriodicAction();
    //            newTime += _periodicTime.TimeInterval;
    //        }

    //        _periodicTime = _periodicTime.WithTime(newTime);
    //    }

    //    public override int GetHashCode() => Id;
    //    public override bool Equals(object obj) => obj is IStatusEffect statusEffect && statusEffect.Id == Id;
    //}
}
