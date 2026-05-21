using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases.Skills
{
    public sealed class SkillStartCooldownUseCase
    {
        private readonly IAbilityRepository _abilityRepository;

        public void Execute(AbilityKey abilityId, float cooldown)
        {
            Ability ability = _abilityRepository.Get(abilityId);
            ability.StartCooldown(cooldown);
            _abilityRepository.Update(ability);
        }
    }
}
