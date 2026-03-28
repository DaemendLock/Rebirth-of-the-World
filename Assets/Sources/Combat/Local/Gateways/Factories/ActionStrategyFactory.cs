using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Gateways.DataSources;

namespace Combat.Local.Gateways.Factories
{
    public class ActionStrategyFactory : IActionStrategyFactory
    {
        private readonly ISkillDataBase _skillDataBase;

        public ActionStrategyFactory(ISkillDataBase skillDataBase)
        {
            _skillDataBase = skillDataBase;
        }

        public IActionStrategy Create(ActionId id)
        {
            IActionStrategy result = null;

            if (_skillDataBase.TryGetActionData(id, out var actionData))
            {
                result = new CastActionStrategy(actionData.FrameData);
            }

            return result;
        }
    }
}
