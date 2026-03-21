using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Factories;
using Combat.Local.Gateways.DataSources;

using System.Collections.Generic;

namespace Temp.Domain.Implementations
{
    public class SkillFactory : ISkillFactory
    {
        private readonly ISkillDataBase _skillDataBase;

        private readonly List<ISkillStrategyFactory> _skillStrategies;

        public SkillFactory(ISkillDataBase skillDataBase, List<ISkillStrategyFactory> skillStrategies)
        {
            _skillDataBase = skillDataBase;
            _skillStrategies = skillStrategies;
        }

        public void RegisterStrategyFactory(ISkillStrategyFactory factory)
        {
            _skillStrategies.Add(factory);
        }

        public Skill Create(SkillId skillId, EntityId? owner)
        {
            SkillFlags flags = _skillDataBase.GetDefaultFlags(skillId);
            IReadOnlyCollection<ActionId> actions = _skillDataBase.GetAssociatedActions(skillId);

            if (actions == null || actions.Count == 0)
            {
                flags |= SkillFlags.Instant;
            }

            ISkillStrategy skillStrategy = GetFactory(skillId)?.Create(skillId, owner);

            return new(skillId, flags, owner, skillStrategy);
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
