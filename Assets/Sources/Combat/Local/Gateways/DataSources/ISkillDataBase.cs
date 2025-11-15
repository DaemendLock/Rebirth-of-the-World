using CastStateSkill;

using Combat.Common.ValueObjects;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Gateways.DataSources
{
    public interface ISkillDataBase
    {
        IFrameData GetFrameData(ActionId id);

        AnimationClip GetAnimation(ActionId id);

        IReadOnlyCollection<ActionId> GetAssociatedActions(SkillId id);
    }
}
