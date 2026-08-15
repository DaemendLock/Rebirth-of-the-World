using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services.Skills;

namespace Combat.Local.Domain.UseCases.Skills
{
    public sealed class SkillStartCooldownUseCase
    {
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly SkillOwnerOperations _skillService;

        public SkillStartCooldownUseCase(SkillOwnerOperations skillService, ISkillOwnerRepository skillOwnerRepository)
        {
            _skillService = skillService;
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

            _skillService.SetCooldown(skillOwner, abilityId.Skill, cooldown);
        }
    }
}
