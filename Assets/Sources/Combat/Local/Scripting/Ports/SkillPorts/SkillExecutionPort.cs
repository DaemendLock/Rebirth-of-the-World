using Combat.API;
using Combat.API.Adapters;
using Combat.API.API.Skills;
using Combat.API.Skills;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Endpoints.Skills;
using Combat.Local.Domain.Repositories.Skill;
using Combat.Local.Scripting.Contexts;

using System.Collections.Generic;

namespace Combat.Local.Scripting.SkillPorts
{
    public sealed class SkillExecutionPort : ISkillExecutionPort
    {
        private readonly Dictionary<SkillId, ISkillScriptNew> _newScripts;
        private readonly Dictionary<AbilityKey, ICastableSkill> _oldScripts;

        private readonly UnitNewAdapter _unitNewAdapter;
        private readonly ISkillMemoryRepository _memoryRepository;

        public bool BeginCast(AbilityKey abilityKey)
        {
            if (_oldScripts.TryGetValue(abilityKey, out var oldScript))
            {
                oldScript.OnCast();
                return oldScript is ICastStateChangeHandler;
            }

            if (_newScripts.TryGetValue(abilityKey.Skill, out var newScript))
            {
                DomainSkillContext domainSkillContext = new(_memoryRepository, abilityKey);

                if (abilityKey.Owner.HasValue == false)
                {
                    newScript.OnCast(null, domainSkillContext);
                }

                var timeline = newScript.OnCast(_unitNewAdapter.Adaptee(abilityKey.Owner.Value) as UnitNew, domainSkillContext);
                return timeline.Startup + timeline.ActiveTime + timeline.Recovery > 0;
            }

            return false;
        }

        public CastFailReason CanCast(AbilityKey abilityKey)
        {
            if (_oldScripts.TryGetValue(abilityKey, out var oldScript))
            {
                return oldScript.CanCast();
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
