using System.Collections.Generic;

using Server.Combat.Data.Entities;
using Server.Combat.Data.Repositories;
using Server.Combat.Domain.Skills;

namespace Server.Combat.Domain.Repositories
{
    public class SkillDataRepository : ISkillDataRepository
    {
        private readonly Dictionary<int, SkillData> _values;

        public SkillDataRepository()
        {
            _values = new();
        }

        public void Add(SkillData skill) => _values[skill.Id] = skill;

        public SkillData Get(SkillId id) => _values.GetValueOrDefault(id.Value, null);

        public void Remove(SkillId id) => _values.Remove(id.Value);
    }
}
