using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;

using UnityEngine;

namespace Combat.Local.Gateways.Repositories
{
    public class SkillAnimationRepository : ISkillAnimationRepository
    {
        private readonly ISkillDataBase _skillDataBase;

        public SkillAnimationRepository(ISkillDataBase skillDataBase)
        {
            _skillDataBase = skillDataBase;
        }

        AnimationClip ISkillAnimationRepository.Get(SkillId id) => _skillDataBase.GetAnimation(id);
    }
}
