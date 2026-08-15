using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.ValueObjects;

using System;

namespace Combat.Local.Domain.UseCases.Skills
{
    public sealed class AbilityProgressAllUseCase
    {
        private readonly ISkillOwnerRepository _ownerRepository;
        private readonly IAbilityRepository _abilityRepository;

        public AbilityProgressAllUseCase(ISkillOwnerRepository ownerRepository, IAbilityRepository abilityRepository)
        {
            _ownerRepository = ownerRepository;
            _abilityRepository = abilityRepository;
        }

        public void Execute(ReadOnlySpan<Updatable> targets, float progressTime)
        {
            foreach (Updatable target in targets)
            {
                ProgressTarget(target.Id, progressTime * target.TimeScale);
            }
        }

        private void ProgressTarget(UnitId target, float deltaTime)
        {
            if (_ownerRepository.TryGet(target, out SkillOwner skillOwner) == false)
            {
                return;
            }

            Span<SkillCooldown> cooldowns = stackalloc SkillCooldown[skillOwner.Cooldowns.Length];
            skillOwner.Cooldowns.CopyTo(cooldowns);

            for (int i = 0; i < cooldowns.Length; i++)
            {
                SkillCooldown cooldown = cooldowns[i];

                if (cooldown.Value <= 0)
                {
                    continue;
                }

                cooldowns[i] = new(cooldown.Skill, cooldown.Value - deltaTime);
            }

            _ownerRepository.Update(new(target, skillOwner.Skills, cooldowns));
        }
    }
}
