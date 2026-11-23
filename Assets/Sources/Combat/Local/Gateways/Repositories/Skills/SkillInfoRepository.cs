using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Domain.Repositories;
using Combat.Domain.ValueObjects;
using Combat.Local.Gateways.DataSources;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories
{
    public class SkillInfoRepository : ISkillInfoRepository
    {
        private readonly ISkillDataBase _skillDataBase;

        public SkillInfoRepository(ISkillDataBase skillDataBase)
        {
            _skillDataBase = skillDataBase;
        }

        public SkillInfo Get(SkillId skillId)
        {
            SkillFlags flags = _skillDataBase.GetDefaultFlags(skillId);
            float cooldown = _skillDataBase.GetBaseCooldown(skillId);
            IReadOnlyCollection<ActionId> actions = _skillDataBase.GetAssociatedActions(skillId);

            if (actions == null || actions.Count == 0)
            {
                flags |= SkillFlags.Instant;
            }

            return new(flags, cooldown, actions);
        }
    }
}
