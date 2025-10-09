using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using UnityEngine;

namespace Combat.Local.Domain.Repositories
{
    public interface ISkillRepository
    {
        Skill Get(SkillId id, EntityId? caster);
    }

    public interface ISkillAnimationRepository
    {
        AnimationClip Get(SkillId id);
    }
}
