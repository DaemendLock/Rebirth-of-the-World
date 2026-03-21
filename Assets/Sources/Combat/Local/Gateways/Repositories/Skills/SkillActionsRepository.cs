using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories.Skills;
using Combat.Local.Gateways.DataSources;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories
{
    public class SkillActionsRepository : ISkillActionsRepository
    {
        private readonly ISkillDataBase _skillDataBase;

        public SkillActionsRepository(ISkillDataBase skillDataBase)
        {
            _skillDataBase = skillDataBase;
        }

        public IReadOnlyCollection<ActionId> Get(SkillId id)
        {
            return _skillDataBase.GetAssociatedActions(id);
        }
    }
}
