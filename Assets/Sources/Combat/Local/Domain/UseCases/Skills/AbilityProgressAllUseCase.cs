using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

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
            SkillOwner skillOwner = _ownerRepository.Get(target);

            Span<(SkillId, float)> cooldowns = skillOwner.Cooldowns;

            for (int i = 0; i < cooldowns.Length; i++)
            {
                var val = cooldowns[i];

                if (val.Item2 <= 0)
                {
                    continue;
                }

                cooldowns[i] = (val.Item1, val.Item2 - deltaTime);
            }

            _ownerRepository.Update(skillOwner);
        }
    }
}
