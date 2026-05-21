using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.Entities.Skills.Effects
{
    public interface ISkillActionStateChangeStrategy
    {
        void Handle(ActionState newState);
    }

    public readonly ref struct SkillActionStateChangeEffect
    {
        private readonly ISkillActionStateChangeStrategy _strategy;

        public SkillActionStateChangeEffect(SkillId skill, ISkillActionStateChangeStrategy strategy)
        {
            Skill = skill;
            _strategy = strategy;
        }

        public SkillId Skill { get; }

        public void Handle(ActionState newState) => _strategy.Handle(newState);
    }
}
