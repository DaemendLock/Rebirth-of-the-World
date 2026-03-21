using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;

using UnityEngine;

namespace Combat.Local.Domain.Repositories
{
    public interface ISkillRepository
    {
        void Create(Skill skill);

        Skill Get(SkillId id, EntityId? caster);
    }

    public interface IActionAnimationRepository
    {
        AnimationClip Get(ActionId id);
    }
}
