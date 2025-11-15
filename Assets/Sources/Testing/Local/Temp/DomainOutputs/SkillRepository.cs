using CastStateSkill;

using Combat.API;
using Combat.API.Controllers;
using Combat.API.Skills;
using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;

using System;

namespace Testing.Local.Temp.DomainOutputs
{
    public class SkillRepository : ISkillRepository
    {
        private readonly SkillApiProvider _skillApiProvider;
        private readonly ISkillDataBase _skillDataBase;

        public SkillRepository(SkillApiProvider skillApiProvider, ISkillDataBase skillDataBase)
        {
            _skillApiProvider = skillApiProvider;
            _skillDataBase = skillDataBase;
        }

        public Skill Get(SkillId skillId, EntityId? casterId)
        {
            SkillApi skillInfo = _skillApiProvider.Get(skillId, casterId);

            if (skillInfo == null)
            {
                return new(skillId, false, Array.Empty<ActionId>(), SkillFlags.None);
            }

            bool canCast = skillInfo.TryGetProperty(out ICastableSkill castable);
            bool startAction = skillInfo.TryGetProperty(out ICastStateChangeHandler actionHandler);
            var actions = _skillDataBase.GetAssociatedActions(skillId);

            return new(skillId, canCast, actions, skillInfo.Flags);
        }
    }
}
