using Combat.Domain.Entities;
using Combat.Domain.Entities.Units;
using Combat.Domain.Repositories;

using System;

namespace Combat.Local.Application.UseCases.Skills
{
    public sealed class SkillOwnerProgressAllUseCase
    {
        private readonly ISkillOwnerRepository _ownerRepository;

        public SkillOwnerProgressAllUseCase(ISkillOwnerRepository ownerRepository)
        {
            _ownerRepository = ownerRepository;
        }

        public void Execute(ReadOnlySpan<Updatable> targets, float progressTime)
        {
            foreach (Updatable target in targets)
            {
                if (_ownerRepository.TryGet(target.Id, out SkillOwner skillOwner) == false)
                {
                    continue;
                }

                skillOwner.ProgressCooldowns(progressTime * target.TimeScale);
            }
        }
    }
}
