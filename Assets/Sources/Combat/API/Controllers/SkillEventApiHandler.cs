using Combat.API.Skills;

using Testing.Local.Temp.DomainOutputs;

namespace Combat.API.Controllers
{
    public class SkillEventApiHandler
    {
        private readonly SkillApiProvider _skillApiProvider;

        public SkillEventApiHandler(SkillApiProvider skillApiProvider)
        {
            _skillApiProvider = skillApiProvider;
        }

        public void HandleCast(SkillCastInfo info)
        {
            SkillApi api = _skillApiProvider.Get(info.SkillId, info.Caster);

            if (api.TryGetProperty(out ICastableSkill script))
            {
                script.OnCast();
            }
        }
    }
}
