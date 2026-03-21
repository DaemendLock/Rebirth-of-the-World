using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;

namespace Combat.API.Controllers
{
    public class SkillApiAdapter
    {
        private readonly ChracterApiAdapter _characterApiProvider;
        private readonly SceneApiAdapter _sceneApiProvider;
        private readonly SkillFacade _skillFacade;

        public SkillApiAdapter(ChracterApiAdapter characterApiProvider, SceneApiAdapter sceneApiProvider, SkillFacade skillFacade)
        {
            _characterApiProvider = characterApiProvider;
            _sceneApiProvider = sceneApiProvider;
            _skillFacade = skillFacade;
        }

        public SkillApi Adaptee(SkillId skillId, EntityId? caster)
        {
            return new(skillId, caster.HasValue ? _characterApiProvider.Adaptee(caster.Value) : null, _skillFacade, _sceneApiProvider.Get());
        }
    }
}
