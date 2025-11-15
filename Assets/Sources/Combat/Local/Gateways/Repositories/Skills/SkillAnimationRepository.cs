using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;

using UnityEngine;

namespace Combat.Local.Gateways.Repositories
{
    public class SkillAnimationRepository : IActionAnimationRepository
    {
        private readonly ISkillDataBase _skillDataBase;

        public SkillAnimationRepository(ISkillDataBase skillDataBase)
        {
            _skillDataBase = skillDataBase;
        }

        AnimationClip IActionAnimationRepository.Get(ActionId id) => _skillDataBase.GetAnimation(id);
    }
}
