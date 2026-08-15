using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.UseCases.Skills
{
    public sealed class SkillStartCooldownUseCase
    {
        private readonly IAbilityRepository _abilityRepository;
        private readonly ISkillOwnerRepository _skillOwnerRepository;

        public SkillStartCooldownUseCase(IAbilityRepository abilityRepository, ISkillOwnerRepository skillOwnerRepository)
        {
            _abilityRepository = abilityRepository;
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

            Span<SkillCooldown> skillCooldowns = stackalloc SkillCooldown[skillOwner.Cooldowns.Length + 1];
            skillOwner.Cooldowns.CopyTo(skillCooldowns);

            for (int i = 0; i < skillCooldowns.Length - 1; i++)
            {
                if (skillCooldowns[i].Skill != abilityId.Skill)
                {
                    continue;
                }

                skillCooldowns[i] = new(skillCooldowns[i].Skill, cooldown);
                _skillOwnerRepository.Update(new(skillOwner.Id, skillOwner.Skills, skillCooldowns[..^1]));
                return;
            }

            skillCooldowns[^1] = new(abilityId.Skill, cooldown);
            _skillOwnerRepository.Update(new(skillOwner.Id, skillOwner.Skills, skillCooldowns));
        }
    }
}
