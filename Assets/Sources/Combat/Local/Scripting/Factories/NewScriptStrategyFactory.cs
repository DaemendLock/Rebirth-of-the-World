using Combat.Common.ValueObjects;
using Combat.Local.Scripting.IDK;
using Combat.Local.Scripting.Idk;
using Combat.API.API.Skills;
using Combat.Local.Scripting.Adapters;
using Combat.Local.Domain.Repositories.Skill;

namespace Combat.Local.Scripting.Factories
{
    public sealed class NewScriptStrategyFactory : ISkillPropertyContainerFactory
    {
        private readonly ISkillMemoryRepository _skillMemoryRepository;
        private readonly UnitNewAdapter _unitNewAdapter;

        public NewScriptStrategyFactory(ISkillMemoryRepository skillMemoryRepository, UnitNewAdapter unitNewAdapter)
        {
            _skillMemoryRepository = skillMemoryRepository;
            _unitNewAdapter = unitNewAdapter;
        }

        public bool CanHandle(SkillId skillId) => true;

        public IAbilityPropertyContainer Create(UnitId? owner, SkillId skillId) =>
            new NewScriptAbilityPropertyContainer(new(owner, skillId), new TestScript(), _unitNewAdapter, _skillMemoryRepository);
    }
}
