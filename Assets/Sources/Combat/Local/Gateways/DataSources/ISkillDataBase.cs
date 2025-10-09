using CastStateSkill;

using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Gateways.DataSources
{
    public interface ISkillDataBase
    {
        IFrameData GetFrameData(SkillId id);

        AnimationClip GetAnimation(SkillId id);
    }
}
