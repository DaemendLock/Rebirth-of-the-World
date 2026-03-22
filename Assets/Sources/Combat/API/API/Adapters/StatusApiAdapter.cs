using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.Repositories;

namespace Combat.API.Adapters
{
    public class StatusApiAdapter
    {
        private readonly CharacterApiAdapter _unitApiProvider;
        private readonly SkillApiAdapter _skillApiProvider;
        private readonly SceneApiAdapter _sceneApiProvider;
        private readonly StatusFacade _statusFacade;

        private readonly IStatusRepository _statusRepository;

        public StatusApiAdapter(CharacterApiAdapter unitApiRepository, SkillApiAdapter skillApiRepository, StatusFacade statusController, SceneApiAdapter sceneApiProvider, IStatusRepository statusRepository)
        {
            _unitApiProvider = unitApiRepository;
            _skillApiProvider = skillApiRepository;
            _sceneApiProvider = sceneApiProvider;
            _statusFacade = statusController;
            _statusRepository = statusRepository;
        }

        public StatusApi Adaptee(StatusId id)
        {
            if (_statusRepository.TryGet(id, out var status) == false)
            {
                return default;
            }

            Unit parentApi = _unitApiProvider.Adaptee(status.Parent);

            SkillApi skill = null;

            if (status.Source.HasValue)
            {
                skill = _skillApiProvider.Adaptee(status.Source.Value, status.Caster);
            }

            return new(id, parentApi, skill, _statusFacade);
        }
    }
}
 