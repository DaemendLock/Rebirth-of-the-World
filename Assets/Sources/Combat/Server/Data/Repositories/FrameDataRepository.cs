using System.Collections.Generic;

using CastStateSkill;

using Server.Combat.Data.Entities;
using Server.Combat.Domain.Repositories;
using Server.Combat.Domain.Skills;

namespace Server.Combat.Data.Repositories
{
    public class FrameDataRepository : IFrameDataRepository
    {
        private readonly Dictionary<int, IFrameData> _cachedFrameData;
        private readonly ISkillDataRepository _skillDataRepository;

        public FrameDataRepository(ISkillDataRepository skillDataRepository)
        {
            _skillDataRepository = skillDataRepository;
            _cachedFrameData = new();
        }

        public void Add(SkillId skillId, IFrameData value) => _cachedFrameData.Add(skillId.Value, value);

        public IFrameData Get(SkillId skillId)
        {
            if (_cachedFrameData.TryGetValue(skillId.Value, out IFrameData result))
            {
                return result;
            }

            SkillData skillData = _skillDataRepository.Get(skillId);

            if (skillData == null)
            {
                return null;
            }

            result = skillData.FrameData;
            _cachedFrameData.Add(skillId.Value, result);
            return result;
        }

        public bool Remove(SkillId key) => _cachedFrameData.Remove(key.Value);
    }
}
