using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.DataSources;

namespace Combat.Local.Gateways.Repositories
{
    public class FrameDataRepository : IFrameDataRepository
    {
        private readonly ISkillDataBase _skillDataBase;

        public FrameDataRepository(ISkillDataBase skillDataBase)
        {
            _skillDataBase = skillDataBase;
        }

        public SkillFrameData Get(SkillId id) => new(id, _skillDataBase.GetFrameData(id));
    }
}
