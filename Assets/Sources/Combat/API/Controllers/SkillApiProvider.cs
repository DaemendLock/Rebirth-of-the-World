using Combat.Common.ValueObjects;

using System.Collections.Generic;

namespace Combat.API.Controllers
{
    public class SkillApiProvider
    {
        private readonly Dictionary<(EntityId?, SkillId), SkillApi> _values;

        public SkillApiProvider()
        {
            _values = new();
        }

        public void Register(SkillApi value) => _values.Add((value.OwnerId, value.SkillId), value);

        public SkillApi Get(SkillId skillId, EntityId? caster) => _values.GetValueOrDefault((caster, skillId));

        public void Remove(SkillId skillId, EntityId? owner) => _values.Remove((owner, skillId));
    }
}
