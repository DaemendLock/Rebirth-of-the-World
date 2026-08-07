using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Gateways.DataSources;

namespace Combat.Local.Gateways.Factories
{
    public class AbilityActionStrategyFactory : IAbilityActionStrategyFactory
    {
        private readonly ISkillDataBase _skillDataBase;

        public AbilityActionStrategyFactory(ISkillDataBase skillDataBase)
        {
            _skillDataBase = skillDataBase;
        }

        public IActionStrategy Create(ActionId id, SkillId source, bool holdable)
        {
            IActionStrategy result = null;

            if (_skillDataBase.TryGetActionData(id, out var actionData))
            {
                result = new FrameDataAbilityActionStrategy(actionData.FrameData, source, holdable);
            }

            return result;
        }
    }
}
