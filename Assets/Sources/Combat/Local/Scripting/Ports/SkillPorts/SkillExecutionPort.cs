using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Capabilities.Skills;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.SkillPorts
{
    public sealed class SkillExecutionPort : ISkillExecutionPort
    {
        private readonly ISkillRuntimeRegistry _runtimeRegistry;
        private readonly UnitNewAdapter _unitNewAdapter;

        public SkillExecutionPort(ISkillRuntimeRegistry runtimeRegistry, UnitNewAdapter unitNewAdapter)
        {
            _runtimeRegistry = runtimeRegistry;
            _unitNewAdapter = unitNewAdapter;
        }

        public bool BeginCast(AbilityKey abilityKey)
        {
            if (_runtimeRegistry.TryGet(abilityKey, out var skillRuntime) == false)
            {
                return false;
            }

            var castable = skillRuntime.Container.GetCapability<ISkillExecuteCapability>();

            if (castable == null)
            {
                return false;
            }

            UnitNew caster = abilityKey.Owner.HasValue ? _unitNewAdapter.Adaptee(abilityKey.Owner.Value) : null;
            DomainCastContext domainCastContext = skillRuntime.CastContexts.Begin(skillRuntime.Context, caster);
            bool changedCurrent = castable.BeginCast(skillRuntime.Context, domainCastContext);

            if (changedCurrent == false)
            {
                skillRuntime.CastContexts.DiscardPending(domainCastContext);
            }

            return changedCurrent;
        }

        public CastFailReason CanCast(AbilityKey abilityKey)
        {
            if (_runtimeRegistry.TryGet(abilityKey, out var skillRuntime) == false)
            {
                return CastFailReason.UnknownSkill;
            }

            var castable = skillRuntime.Container.GetCapability<ISkillExecuteCapability>();

            if (castable == null)
            {
                return CastFailReason.NotCastable;
            }

            return castable.CanCast(skillRuntime.Context);
        }
    }
}
