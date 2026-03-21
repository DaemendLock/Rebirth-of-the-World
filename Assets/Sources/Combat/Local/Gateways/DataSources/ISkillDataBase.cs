using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Data.Models;

using System.Collections.Generic;

namespace Combat.Local.Gateways.DataSources
{
    public interface ISkillDataBase
    {
        bool TryGetActionData(ActionId id, out ActionData value);

        IReadOnlyCollection<ActionId> GetAssociatedActions(SkillId id);

        SkillFlags GetDefaultFlags(SkillId id);
    }
}
