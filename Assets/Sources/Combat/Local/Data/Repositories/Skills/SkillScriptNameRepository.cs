using System.Collections.Generic;

using Combat.Common.ValueObjects;
using Combat.Local.Data.Repositories;

namespace Combat.Local.Domain.Repositories
{
    public class SkillScriptNameRepository : ISkillScriptNameRepository
    {
        private readonly Dictionary<SkillId, string> _values;

        public SkillScriptNameRepository()
        {
            _values = new();
        }

        public void Add(SkillId id, string name) => _values.Add(id, name);
        public void Delete(SkillId id) => _values.Remove(id);
        public string Get(SkillId id) => _values.GetValueOrDefault(id, null);
    }
}
