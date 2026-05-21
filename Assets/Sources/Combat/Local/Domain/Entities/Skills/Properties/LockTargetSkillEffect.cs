using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Skills.Effects
{
    public interface ILockTargetStrategy
    {
        bool Handle(UnitId entityId);
    }

    public readonly ref struct LockTargetSkillEffect
    {
        private readonly ILockTargetStrategy _strategy;

        public LockTargetSkillEffect(SkillId skillId, ILockTargetStrategy strategy)
        {
            _strategy = strategy;
            SkillId = skillId;
        }

        public SkillId SkillId { get; }

        public bool CanTarget(UnitId entityId) => _strategy.Handle(entityId);
    }
}
