using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.ValueObjects
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
    }
}
