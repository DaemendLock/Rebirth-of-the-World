using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Repositories;

namespace Combat.Local.Data.Repositories
{
    public class SkillRepository : ISkillRepository
    {
        private readonly Dictionary<SkillId, Skill> _values;

        public SkillRepository()
        {
            _values = new();
        }

        public void Create(Skill value) => _values.Add(value.Id, value);
        public Skill Get(SkillId id) => _values[id];
        public void Update(Skill value) => _values[value.Id] = value;
        public void Delete(SkillId id) => _values.Remove(id);

        public bool Contains(SkillId id) => _values.ContainsKey(id);
    }
}
