using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Gateways.DataSources;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Factories
{
    public class AbilityFactory : IAbilityFactory
    {
        private readonly ISkillDataBase _skillDataBase;

        private readonly List<ISkillStrategyFactory> _skillStrategies;

        public AbilityFactory(ISkillDataBase skillDataBase, List<ISkillStrategyFactory> skillStrategies)
        {
            _skillDataBase = skillDataBase;
            _skillStrategies = skillStrategies;
        }

        public void RegisterStrategyFactory(ISkillStrategyFactory factory)
        {
            _skillStrategies.Add(factory);
        }

        public Ability Create(SkillId skillId, UnitId? owner)
        {
            SkillFlags flags = _skillDataBase.GetDefaultFlags(skillId);
            IReadOnlyCollection<ActionId> actions = _skillDataBase.GetAssociatedActions(skillId);

            if (actions == null || actions.Count == 0)
            {
                flags |= SkillFlags.Instant;
            }

            IAbilityPropertyContainer properties = GetFactory(skillId)?.Create(owner, skillId);

            return new(skillId, flags, owner, actions, properties);
        }

        private ISkillStrategyFactory GetFactory(SkillId id)
        {
            foreach (ISkillStrategyFactory factory in _skillStrategies)
            {
                if (factory.CanHandle(id) == false)
                {
                    continue;
                }

                return factory;
            }

            return null;
        }
    }
}
