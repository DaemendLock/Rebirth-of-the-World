using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

using System;

namespace Combat.Local.Domain.UseCases.Skills
{
    public sealed class AbilityUpdateAllUseCase
    {
        private readonly ISkillOwnerRepository _ownerRepository;
        private readonly IAbilityRepository _abilityRepository;

        public AbilityUpdateAllUseCase(ISkillOwnerRepository ownerRepository, IAbilityRepository abilityRepository)
        {
            _ownerRepository = ownerRepository;
            _abilityRepository = abilityRepository;
        }

        public void Execute(ReadOnlySpan<Updatable> targets, float deltaTime)
        {
            foreach (Updatable target in targets)
            {
                UpdateTarget(target.Id, deltaTime * target.TimeScale);
            }
        }

        private void UpdateTarget(UnitId target, float deltaTime)
        {
            var abilityOwner = _ownerRepository.Get(target);

            foreach (SkillId id in abilityOwner.GetAll())
            {
                Ability ability = _abilityRepository.Get(new(target, id));
                ability.ProgressCooldown(deltaTime);
                _abilityRepository.Update(ability);
            }
        }
    }
}
