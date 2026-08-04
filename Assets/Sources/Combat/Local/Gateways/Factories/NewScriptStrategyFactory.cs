using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Domain.Repositories.Skill;
using Combat.Local.Gateways.Repositories.Skills;

namespace Combat.Local.Gateways.Factories
{
    public sealed class NewScriptStrategyFactory : ISkillStrategyFactory
    {
        private readonly ISkillMemoryRepository _skillMemoryRepository;

        public NewScriptStrategyFactory(ISkillMemoryRepository skillMemoryRepository)
        {
            _skillMemoryRepository = skillMemoryRepository;
        }

        public bool CanHandle(SkillId skillId) => true;

        public IAbilityPropertyContainer Create(UnitId? owner, SkillId skillId) =>
            new NewScriptAbilityPropertyContainer(new(owner, skillId), new(), _skillMemoryRepository);
    }
}
