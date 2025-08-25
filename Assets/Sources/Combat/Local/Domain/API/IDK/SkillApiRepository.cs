using System.Collections.Generic;

using Combat.Common.ValueObjects;

namespace Combat.Local.Domain.API.IDK
{
    public class SkillApiRepository
    {
        private readonly Dictionary<(EntityId, SkillId), ScriptedSkill> _values;

        public SkillApiRepository()
        {
            _values = new();
        }

        public void Create(ScriptedSkill value) => _values.Add((value.Caster.Id, value.Id), value);

        public ScriptedSkill Get(EntityId caster, SkillId skillId) => _values.GetValueOrDefault((caster, skillId));
    }
}
