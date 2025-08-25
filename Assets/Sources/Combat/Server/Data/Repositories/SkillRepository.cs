using System.Collections.Generic;

using Server.Combat.Domain.Entities;
using Server.Combat.Domain.Repositories;
using Server.Combat.Domain.Skills;

namespace Server.Combat.Data.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly Dictionary<int, Skill> _values;

        public SkillRepository()
        {
            _values = new();
        }

        public void Add(Skill skill) => _values.Add(skill.Id.Value, skill);

        public Skill Get(SkillId skillId) => _values.GetValueOrDefault(skillId.Value, null);

        public void Remove(SkillId skillId) => _values.Remove(skillId.Value);
    }
}
