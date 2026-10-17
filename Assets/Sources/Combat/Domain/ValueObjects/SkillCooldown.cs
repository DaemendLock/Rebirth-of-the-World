using Combat.Common.Primitives;

namespace Combat.Local.Application.ValueObjects
{
    public readonly struct SkillCooldown
    {
        public SkillCooldown(SkillId skill, float currentValue)
        {
            Skill = skill;
            Value = currentValue;
        }

        public SkillId Skill { get; }
        public float Value { get; }

        public SkillCooldown Progress(float deltaTime) => new(Skill, Value - deltaTime);
    }
}
