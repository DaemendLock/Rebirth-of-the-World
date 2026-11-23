using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

namespace Combat.Local.Application.Endpoints.Skills
{
    public readonly struct CastInfo
    {
        public CastInfo(UnitId caster, SkillId skillId, SkillFlags flags, ActionId? action)
        {
            Caster = caster;
            SkillId = skillId;
            Flags = flags;
            RequestedAction = action;
        }

        public UnitId Caster { get; }
        public SkillId SkillId { get; }
        public SkillFlags Flags { get; }
        public ActionId? RequestedAction { get; }
    }

    public interface ISkillExecutionPort
    {
        CastFailReason CanCast(CastInfo castInfo);
        ActionFlags BeginCast(CastInfo castInfo);
    }
}
