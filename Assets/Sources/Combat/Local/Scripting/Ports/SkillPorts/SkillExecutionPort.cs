using Combat.API;
using Combat.API.API.IDK;
using Combat.API.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Domain.Repositories.Skill;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Scripting.Contexts;
using Combat.Local.Scripting.Idk.Capabilities.Skills;

using System.Collections.Generic;

namespace Combat.Local.Scripting.SkillPorts
{

    public sealed class SkillExecutionPort : ISkillExecutionPort
    {
        private readonly Dictionary<SkillId, ISkillScriptNew> _newScripts;

        private readonly ISkillRuntimeRegistry _runtimeRegistry;

        private readonly UnitNewAdapter _unitNewAdapter;
        private readonly ISkillMemoryRepository _memoryRepository;

        public SkillExecutionPort(UnitNewAdapter unitNewAdapter, ISkillMemoryRepository memoryRepository, ISkillRuntimeRegistry runtimeRegistry)
        {
            _unitNewAdapter = unitNewAdapter;
            _memoryRepository = memoryRepository;
            _newScripts = new();
            _runtimeRegistry = runtimeRegistry;
        }

        public bool BeginCast(AbilityKey abilityKey)
        {
            if (_runtimeRegistry.TryGet(abilityKey, out var oldScript))
            {
                if (oldScript.TryGet(out ExecuteSkillCapability castable) == false)
                {
                    return false;
                }

                castable.BeginCast();
                return true;
            }

            if (_newScripts.TryGetValue(abilityKey.Skill, out var newScript))
            {
                DomainSkillContext domainSkillContext = new(_memoryRepository, abilityKey);

                return false;
            }

            return false;
        }

        public CastFailReason CanCast(AbilityKey abilityKey)
        {
            if (_runtimeRegistry.TryGet(abilityKey, out var oldScript))
            {
                if (oldScript.TryGet(out ExecuteSkillCapability castable) == false)
                {
                    return CastFailReason.CantCast;
                }

                return castable.CanCast();
            }

            if (_newScripts.TryGetValue(abilityKey.Skill, out var newScript))
            {
                DomainSkillContext domainSkillContext = new(_memoryRepository, abilityKey);

                if (abilityKey.Owner.HasValue == false)
                {
                    newScript.OnCast(null, domainSkillContext);
                }

                newScript.OnCast(_unitNewAdapter.Adaptee(abilityKey.Owner.Value) as UnitNew, domainSkillContext);
            }

            return CastFailReason.UnknownSkill;
        }
    }
}
