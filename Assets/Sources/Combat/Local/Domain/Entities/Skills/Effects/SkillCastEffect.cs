using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Skills.Effects
{
    public interface ISkillCastStrategy
    {
        CastFailReason CanCast(EntityId? caster);
        void Execute(EntityId? caster);
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

        public CastFailReason CanCast(EntityId? caster) => _skillCastStrategy.CanCast(caster);

        public void Execute(EntityId? caster) => _skillCastStrategy.Execute(caster);
    }
}
