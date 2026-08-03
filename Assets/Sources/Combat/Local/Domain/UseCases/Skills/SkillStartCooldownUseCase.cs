using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases.Skills
{
    public sealed class SkillStartCooldownUseCase
    {
        private readonly IAbilityRepository _abilityRepository;
        private readonly ISkillOwnerRepository _skillOwnerRepository;

        public void Execute(AbilityKey abilityId, float cooldown)
        {
            if (abilityId.Owner.HasValue == false)
            {
                throw new System.NotImplementedException();
            }

            var skillOwner = _skillOwnerRepository.Get(abilityId.Owner.Value);
            Ability ability = _abilityRepository.Get(abilityId);
            //ability.StartCooldown(cooldown);
            _abilityRepository.Update(ability);
        }
    }
}
