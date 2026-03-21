using Combat.Common.ValueObjects;

using System.Collections.Generic;

namespace Combat.Local.Domain.Repositories.Skills
{
    public interface ISkillActionsRepository
    {
        IReadOnlyCollection<ActionId> Get(SkillId id);
    }
}
