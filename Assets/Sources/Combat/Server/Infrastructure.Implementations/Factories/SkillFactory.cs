using Server.Combat.Data.Entities;
using Server.Combat.Data.Repositories;
using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Skills;
using Server.Combat.Infrastructure.Factories;

namespace Server.Combat.Infrastructure.Implementations.Factories
{
    public class SkillFactory : ISkillFactory
    {
        private readonly ISkillDataRepository _skillDataRepository;

        public SkillFactory(ISkillDataRepository skillDataRepository)
        {
            _skillDataRepository = skillDataRepository;
        }

        public Skill Create(SkillId id)
        {
            SkillData skillData = _skillDataRepository.Get(id);

            if (skillData == null)
            {
                throw new System.ArgumentException($"No data found to create skill {id}");
            }

            return new Skill(id, skillData.Flags, skillData.Cooldown, skillData.FrameData);
        }
    }
}
