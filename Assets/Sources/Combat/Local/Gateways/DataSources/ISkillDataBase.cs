using CastStateSkill;

using Combat.Common.Flags;
using Combat.Common.Primitives;

using System;
using System.Collections.Generic;

namespace Combat.Local.Gateways.DataSources
{
    public interface ISkillDataBase
    {
        bool TryGetFrameData(ActionId id, out IFrameData value);

        IReadOnlyCollection<ActionId> GetAssociatedActions(SkillId id);

        SkillFlags GetDefaultFlags(SkillId id);
        float GetBaseCooldown(SkillId skillId);
        Type GetScriptType(SkillId id);
        global::Data.Entities.SkillData Get(SkillId id);
    }
}
