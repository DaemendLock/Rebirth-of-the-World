using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;
using Combat.Local.Gateways.Models;

using System.Collections.Generic;

namespace Combat.Local.Gateways.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly Dictionary<SkillId, SkillData> _values;

        public SkillRepository()
        {
            _values = new();
        }

        public void Create(Skill skill)
        {
            SkillFlags flags = skill.Flags;
            ISkillStrategy skillStrategy = skill.Strategy;

            _values[skill.Id] = new(skillStrategy, flags);
        }

        public Skill Get(SkillId skillId, EntityId? ownerId)
        {
            if (_values.TryGetValue(skillId, out SkillData data) == false)
            {
                throw new System.InvalidOperationException();
            }

            return new(skillId, data.Flags, ownerId, data.Strategy);
        }
    }
}
