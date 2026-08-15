using Combat.Common.Flags;
using Combat.Common.Primitives;
using Combat.Local.Gateways.Models;

using System;
using System.Collections.Generic;

namespace Combat.Local.Gateways.DataSources
{
    public interface ISkillDataBase
    {
        bool TryGetActionData(ActionId id, out ActionData value);

        IReadOnlyCollection<ActionId> GetAssociatedActions(SkillId id);

        SkillFlags GetDefaultFlags(SkillId id);
        Type GetScriptType(SkillId id);
        global::Data.Entities.SkillData Get(SkillId id);
    }
}
