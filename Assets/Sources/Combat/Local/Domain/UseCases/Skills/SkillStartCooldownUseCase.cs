using Combat.Common.Primitives;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases.Skills
{
    public sealed class SkillStartCooldownUseCase
    {
        private readonly ISkillOwnerRepository _skillOwnerRepository;

        public SkillStartCooldownUseCase(ISkillOwnerRepository skillOwnerRepository)
        {
            _skillOwnerRepository = skillOwnerRepository;
        }

        public void Execute(AbilityKey abilityId, float cooldown)
        {
            if (abilityId.Owner.HasValue == false)
            {
                throw new System.InvalidOperationException("Unable to set cooldown of null unit.");
            }

            if (_skillOwnerRepository.TryGet(abilityId.Owner.Value, out SkillOwner skillOwner) == false)
            {
                return;
            }

            skillOwner.SetCooldown(abilityId.Skill, cooldown);
        }
    }
}
