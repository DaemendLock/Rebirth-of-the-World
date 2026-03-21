using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.Facades
{
    public readonly struct SkillFacade
    {
        private readonly ISkillRepository _skillRepository;

        public SkillFacade(ISkillRepository skillRepository)
        {
            _skillRepository = skillRepository;
        }

        public SkillFlags GetFlags(SkillId id, EntityId? owner)
        {
            return _skillRepository.Get(id, owner).Flags;
        }
    }
}
