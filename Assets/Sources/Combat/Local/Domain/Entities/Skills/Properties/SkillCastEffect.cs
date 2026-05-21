using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Skills.Effects
{
    public interface ISkillCastStrategy
    {
        CastFailReason CanCast();
        void Execute();
    }

    public readonly ref struct SkillCastEffect
    {
        private readonly ISkillCastStrategy _skillCastStrategy;

        public SkillCastEffect(SkillId skill, ISkillCastStrategy skillCastStrategy)
        {
            Skill = skill;
            _skillCastStrategy = skillCastStrategy;
        }

        public SkillId Skill { get; }

        public CastFailReason CanCast() => _skillCastStrategy.CanCast();

        public void Execute() => _skillCastStrategy.Execute();
    }
}
