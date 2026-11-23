using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Application.Endpoints.Skills;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Capabilities.Skills;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.SkillPorts
{
    public sealed class SkillExecutionPort : ISkillExecutionPort
    {
        private readonly ISkillRuntimeRegistry _skillScriptRegistry;
        private readonly ICastRuntimeRegistry _castRuntimeRegistry;
        private readonly UnitNewAdapter _unitNewAdapter;

        public SkillExecutionPort(ISkillRuntimeRegistry runtimeRegistry, ICastRuntimeRegistry castRuntimeRegistry, UnitNewAdapter unitNewAdapter)
        {
            _skillScriptRegistry = runtimeRegistry;
            _castRuntimeRegistry = castRuntimeRegistry;
            _unitNewAdapter = unitNewAdapter;
        }

        public ActionFlags BeginCast(CastInfo castInfo)
        {
            if (_skillScriptRegistry.TryGet(new(castInfo.Caster, castInfo.SkillId), out var skillRuntime) == false)
            {
                throw new System.InvalidOperationException("Script is not registered");
            }

            ISkillExecuteCapability castable = skillRuntime.Container.GetCapability<ISkillExecuteCapability>()
                ?? throw new System.InvalidOperationException("Script is not castable");
            DomainCastContext domainCastContext = new(null, _unitNewAdapter.Adaptee(castInfo.Caster));

            castable.BeginCast(skillRuntime.Context, domainCastContext);

            if (castInfo.RequestedAction.HasValue == false)
            {
                return ActionFlags.None;
            }

            _castRuntimeRegistry.Create(new(castInfo.Caster, castInfo.SkillId), new(domainCastContext, skillRuntime.Container));
            return ParseFlags(castInfo);
        }

        public CastFailReason CanCast(CastInfo castInfo)
        {
            if (_skillScriptRegistry.TryGet(new(castInfo.Caster, castInfo.SkillId), out SkillRuntime skillRuntime) == false)
            {
                return CastFailReason.UnknownSkill;
            }

            ISkillExecuteCapability castable = skillRuntime.Container.GetCapability<ISkillExecuteCapability>();

            if (castable == null)
            {
                return CastFailReason.NotCastable;
            }

            return castable.CanCast(skillRuntime.Context);
        }

        private ActionFlags ParseFlags(CastInfo castInfo)
        {
            ActionFlags flags = ActionFlags.None;

            if (castInfo.Flags.HasFlag(SkillFlags.DontRestrictMovement))
            {
                flags |= ActionFlags.AllowMovement;
            }

            if (castInfo.Flags.HasFlag(SkillFlags.CanHold))
            {
                flags |= ActionFlags.Holdable;
            }

            return flags;
        }
    }
}
