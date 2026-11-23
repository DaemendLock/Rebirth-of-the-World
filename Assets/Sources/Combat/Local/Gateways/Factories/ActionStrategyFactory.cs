using CastStateSkill;

using Combat.Common.Primitives;
using Combat.Domain.ValueObjects;
using Combat.Local.Application.Factories;
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

        public CastActivity Create(ActionId id, SkillId source, bool holdable)
        {
            if (_skillDataBase.TryGetFrameData(id, out IFrameData frameData) == false)
            {
                throw new System.InvalidOperationException();
            }

            CastActivity castActivity = new(source, frameData);

            if (holdable == false)
            {
                castActivity.Release();
            }

            return castActivity;
        }
    }
}
