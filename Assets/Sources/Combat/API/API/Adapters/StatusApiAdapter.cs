using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.Repositories;

namespace Combat.API.Adapters
{
    public readonly struct StatusApiAdapter
    {
        private readonly CharacterApiAdapter _unitApiProvider;
        private readonly AbilityApiAdapter _skillApiProvider;
        private readonly SceneApiAdapter _sceneApiProvider;
        private readonly StatusFacade _statusFacade;

        private readonly IStatusRepository _statusRepository;

        public StatusApiAdapter(CharacterApiAdapter unitApiRepository, AbilityApiAdapter skillApiRepository, StatusFacade statusController, SceneApiAdapter sceneApiProvider, IStatusRepository statusRepository)
        {
            _unitApiProvider = unitApiRepository;
            _skillApiProvider = skillApiRepository;
            _sceneApiProvider = sceneApiProvider;
            _statusFacade = statusController;
            _statusRepository = statusRepository;
        }

        public StatusApi Adaptee(StatusId id, UnitId parent, AbilityKey? abilityKey)
        {
            Unit parentApi = _unitApiProvider.Adaptee(parent);

            AbilityApi skill = null;

            if (abilityKey.HasValue)
            {
                skill = _skillApiProvider.Adaptee(abilityKey.Value);
            }

            return new(id, parentApi, skill, _statusFacade);
        }
    }
}
