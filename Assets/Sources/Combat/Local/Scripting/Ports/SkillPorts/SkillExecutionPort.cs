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
            if (_runtimeRegistry.TryGet(abilityKey, out var oldScript) == false)
            {
                return false;
            }

            if (oldScript.TryGet(out ExecuteSkillCapability castable) == false)
            {
                return false;
            }

            return castable.BeginCast(); ;
        }

        public CastFailReason CanCast(AbilityKey abilityKey)
        {
            if (_runtimeRegistry.TryGet(abilityKey, out var oldScript) == false)
            {
                return CastFailReason.UnknownSkill;
            }

            if (oldScript.TryGet(out ExecuteSkillCapability castable) == false)
            {
                return CastFailReason.CantCast;
            }

            return castable.CanCast();
        }
    }
}
