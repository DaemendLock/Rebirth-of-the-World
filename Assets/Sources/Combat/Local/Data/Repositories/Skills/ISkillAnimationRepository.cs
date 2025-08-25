using Combat.Common.ValueObjects;

using UnityEngine;

namespace Combat.Local.Data.Repositories
{
    public interface ISkillAnimationRepository
    {
        void Create(SkillId id, AnimationClip clip);
        void Update(SkillId id, AnimationClip clip);
        AnimationClip Get(SkillId id);
        void Delete(SkillId id);
    }
}
