using Combat.Common.Primitives;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Scripting.Capabilities.Skills;
using Combat.Local.Scripting.Runtime;

namespace Combat.Local.Scripting.SkillPorts
{

    public sealed class SkillExecutionPort : ISkillExecutionPort
    {
        private readonly ISkillRuntimeRegistry _runtimeRegistry;

        public SkillExecutionPort(ISkillRuntimeRegistry runtimeRegistry)
        {
            _runtimeRegistry = runtimeRegistry;
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

            return castable.BeginCast(skillRuntime.Context);
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
