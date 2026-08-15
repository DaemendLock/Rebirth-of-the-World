using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Scripting.Adapters;

namespace Combat.Local.Scripting.Capabilities.Skills
{
    public readonly ref struct EvaluateSkillTargetCapability
    {
        private readonly ITargettableSkill _skill;
        private readonly CharacterApiAdapter _characterApiAdapter;

        public EvaluateSkillTargetCapability(ITargettableSkill skill, CharacterApiAdapter characterApiAdapter)
        {
            _skill = skill;
            _characterApiAdapter = characterApiAdapter;
        }

        public bool CanTarget(UnitId target) => _skill.CanTarget(_characterApiAdapter.Adaptee(target));
    }
}
