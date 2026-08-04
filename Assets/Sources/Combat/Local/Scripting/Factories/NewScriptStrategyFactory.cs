using Combat.API;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Factories;
using Combat.Local.Scripting.Idk;

namespace Combat.Local.Scripting.Factories
{
    public sealed class NewScriptStrategyFactory : ISkillPropertyContainerFactory
    {
        public NewScriptStrategyFactory()
        {
        }

        public bool CanHandle(SkillId skillId) => true;

        public IAbilityPropertyContainer Create(UnitId? owner, SkillId skillId) =>
            new NewScriptAbilityPropertyContainer(new(owner, skillId));
    }
}
