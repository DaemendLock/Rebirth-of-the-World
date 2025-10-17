using Combat.API.Skills;
using Combat.Common.ValueObjects;

using System.Collections.Generic;

using Testing.Local.Temp.DomainOutputs;

namespace Combat.API.Controllers
{
    public class SkillCastEventApiHandler
    {
        private readonly SkillApiProvider _skillApiProvider;

        public void Handle(SkillCastInfo info)
        {
            SkillApi api = _skillApiProvider.Get(info.SkillId, info.Caster);

            if (api.TryGetProperty(out ICastableSkill script))
            {
                script.OnCast();
            }
        }
    }

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
