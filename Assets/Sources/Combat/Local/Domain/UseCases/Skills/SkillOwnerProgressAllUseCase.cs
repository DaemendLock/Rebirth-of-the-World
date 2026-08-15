using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;
using Combat.Local.Domain.Services.Skills;

using System;

namespace Combat.Local.Domain.UseCases.Skills
{
    public sealed class SkillOwnerProgressAllUseCase
    {
        private readonly ISkillOwnerRepository _ownerRepository;
        private readonly SkillOwnerOperations _skillOwnerOperations;

        public SkillOwnerProgressAllUseCase(ISkillOwnerRepository ownerRepository, SkillOwnerOperations skillOwnerOperations)
        {
            _ownerRepository = ownerRepository;
            _skillOwnerOperations = skillOwnerOperations;
        }

        public void Execute(ReadOnlySpan<Updatable> targets, float progressTime)
        {
            foreach (Updatable target in targets)
            {
                if (_ownerRepository.TryGet(target.Id, out SkillOwner skillOwner) == false)
                {
                    continue;
                }

                _skillOwnerOperations.Progress(skillOwner, progressTime * target.TimeScale);
            }
        }
    }
}
