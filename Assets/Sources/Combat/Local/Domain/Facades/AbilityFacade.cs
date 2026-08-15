using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.Facades
{
    public readonly struct AbilityFacade
    {
        private readonly IAbilityRepository _abilityRepository;

        public AbilityFacade(IAbilityRepository skillRepository)
        {
            _abilityRepository = skillRepository;
        }

        public SkillFlags GetFlags(UnitId? owner, SkillId id)
        {
            return _abilityRepository.Get(new(owner, id)).Flags;
        }
    }
}
