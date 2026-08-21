using Combat.API.Contexts;
using Combat.API.Scripting;
using Combat.API.Skills;
using Combat.Common.ValueObjects;

namespace Combat.Local.Scripting.Capabilities.Skills
{
    public interface ISkillExecuteCapability
    {
        CastFailReason CanCast(ISkillContext context);
        bool BeginCast(ISkillContext skillContext, ICastContext castContext);
    }

    public sealed class OldScriptSkillExecuteContext : ISkillExecuteCapability
    {
        private readonly ICastableSkill _castableSkill;

        public OldScriptSkillExecuteContext(ICastableSkill castableSkill)
        {
            _castableSkill = castableSkill;
        }

        public CastFailReason CanCast(ISkillContext context) => _castableSkill.CanCast();

        public bool BeginCast(ISkillContext skillContext, ICastContext castContext) => _castableSkill.OnCast();
    }

    public sealed class NewSkillExecuteCapability : ISkillExecuteCapability
    {
        private readonly ICastableNew _script;

        public NewSkillExecuteCapability(ICastableNew script)
        {
            _script = script;
        }

        public bool BeginCast(ISkillContext skillContext, ICastContext castContext) => _script.OnCast(skillContext, castContext);
        public CastFailReason CanCast(ISkillContext context) => CastFailReason.Success;
    }
}
