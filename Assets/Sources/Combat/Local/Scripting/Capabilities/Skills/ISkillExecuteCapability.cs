using Combat.API;
using Combat.API.Contexts;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Common.ValueObjects;

namespace Combat.Local.Scripting.Capabilities.Skills
{
    public interface ISkillExecuteCapability
    {
        CastFailReason CanCast(ISkillContext context);
        bool BeginCast(ISkillContext skillContext);
    }

    public sealed class OldScriptSkillExecuteContext : ISkillExecuteCapability
    {
        private readonly ICastableSkill _castableSkill;

        public OldScriptSkillExecuteContext(ICastableSkill castableSkill)
        {
            _castableSkill = castableSkill;
        }

        public CastFailReason CanCast(ISkillContext context) => _castableSkill.CanCast();

        public bool BeginCast(ISkillContext skillContext) => _castableSkill.OnCast();
    }

    public sealed class NewSkillExecuteCapability : ISkillExecuteCapability
    {
        private readonly ISkillScriptNew _skillScript;
        private readonly UnitNew _unitNew;

        public NewSkillExecuteCapability(ISkillScriptNew skillScript, UnitNew unitNew)
        {
            _skillScript = skillScript;
            _unitNew = unitNew;
        }

        public bool BeginCast(ISkillContext skillContext) => _skillScript.OnCast(_unitNew, skillContext);
        public CastFailReason CanCast(ISkillContext context) => CastFailReason.Success;
    }
}
