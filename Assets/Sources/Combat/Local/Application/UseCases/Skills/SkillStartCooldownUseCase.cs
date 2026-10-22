using Combat.Common.Primitives;
using Combat.Domain.Entities.Units;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Application.UseCases.Skills
{
    public sealed class SkillStartCooldownUseCase
    {
        private readonly IUnitStore _unitStore;

        public SkillStartCooldownUseCase(IUnitStore unitStore)
        {
            _unitStore = unitStore;
        }

        public void Execute(AbilityKey abilityId, float cooldown)
        {
            if (abilityId.Owner.HasValue == false)
            {
                throw new System.InvalidOperationException("Unable to set cooldown of null unit.");
            }

            _unitStore.Get(abilityId.Owner.Value).Skills.SetCooldown(abilityId.Skill, cooldown);
        }
    }
}
