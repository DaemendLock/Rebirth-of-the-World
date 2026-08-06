using Combat.API.Skills;
using Combat.Common.ValueObjects;

namespace Combat.Local.Scripting.Idk.Capabilities.Skills
{
    public readonly ref struct ExecuteSkillCapability
    {
        private readonly ICastableSkill _skill;

        public ExecuteSkillCapability(ICastableSkill skill)
        {
            _skill = skill;
        }

        public CastFailReason CanCast() => _skill.CanCast();

        public void BeginCast() => _skill.OnCast();
    }
}
