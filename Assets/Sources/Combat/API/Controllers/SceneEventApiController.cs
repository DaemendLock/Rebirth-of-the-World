using Combat.API.Controllers.Factories;
using Combat.Local.Events;

namespace Combat.API.Controllers
{
    public class SceneEventApiController
    {
        private readonly UnitApiProvider _unitApiProvider;
        private readonly SkillApiProvider _skillApiProvider;

        private readonly IUnitApiFactory _unitApiFactory;
        private readonly ISkillApiFactory _skillApiFactory;

        public SceneEventApiController(UnitApiProvider unitApiProvider, SkillApiProvider skillApiProvider, IUnitApiFactory unitApiFactory, ISkillApiFactory skillApiFactory)
        {
            _unitApiProvider = unitApiProvider;
            _skillApiProvider = skillApiProvider;
            _unitApiFactory = unitApiFactory;
            _skillApiFactory = skillApiFactory;
        }

        public void HandleCharacterCreated(CharacterCreatedInfo info)
        {
            Unit unitApi = _unitApiFactory.Create(info.Id);
            _unitApiProvider.Register(unitApi);

            foreach (var skill in info.InitialSkills)
            {
                if (_skillApiProvider.Get(skill, info.Id) != null)
                {
                    return;
                }

                var value = _skillApiFactory.Create(skill, info.Id);
                _skillApiProvider.Register(value);
            }
        }
    }
}
