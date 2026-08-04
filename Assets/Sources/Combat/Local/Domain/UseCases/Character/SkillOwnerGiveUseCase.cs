using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Domain.UseCases.Character
{
    public sealed class SkillOwnerGiveUseCase
    {
        private readonly ISkillOwnerRepository _skillOwnerRepository;
        private readonly IAbilityFactory _abilityFactory;
        private readonly IAbilityRepository _abilityRepository;

        public void Execute(UnitId target, SkillId skillId)
        {
            SkillOwner skillOwner = _skillOwnerRepository.Get(target);
            Ability ability = _abilityFactory.Create(skillId, target);
            _abilityRepository.Create(ability);
            //ability.Properties.Give();
        }
    }
}
