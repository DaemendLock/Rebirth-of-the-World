using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Gateways.DataSources;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Factories
{
    public class AbilityFactory : IAbilityFactory
    {
        private readonly ISkillDataBase _skillDataBase;

        public AbilityFactory(ISkillDataBase skillDataBase)
        {
            _skillDataBase = skillDataBase;
        }

        public SkillInfo Create(SkillId skillId)
        {
            SkillFlags flags = _skillDataBase.GetDefaultFlags(skillId);
            float cooldown = _skillDataBase.GetBaseCooldown(skillId);
            IReadOnlyCollection<ActionId> actions = _skillDataBase.GetAssociatedActions(skillId);

            if (actions == null || actions.Count == 0)
            {
                flags |= SkillFlags.Instant;
            }

            return new(skillId, flags, cooldown, actions);
        }
    }
}
