using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.Repositories;

namespace Combat.API.Adapters
{
    public sealed class StatusApiAdapter : IStatusApiAdapter
    {
        private readonly ICharacterApiAdapter _unitApiProvider;
        private readonly IAbilityApiAdapter _skillApiProvider;
        private readonly ISceneApiAdapter _sceneApiProvider;
        private readonly StatusFacade _statusFacade;

        private readonly IStatusRepository _statusRepository;

        public StatusApiAdapter(ICharacterApiAdapter unitApiRepository, IAbilityApiAdapter skillApiRepository, StatusFacade statusController, ISceneApiAdapter sceneApiProvider, IStatusRepository statusRepository)
        {
            _unitApiProvider = unitApiRepository;
            _skillApiProvider = skillApiRepository;
            _sceneApiProvider = sceneApiProvider;
            _statusFacade = statusController;
            _statusRepository = statusRepository;
        }

        public IStatusApi Adaptee(StatusId id, UnitId parent, AbilityKey? abilityKey)
        {
            IUnit parentApi = _unitApiProvider.Adaptee(parent);

            IAbilityApi skill = null;

            if (abilityKey.HasValue)
            {
                skill = _skillApiProvider.Adaptee(abilityKey.Value);
            }

            return new StatusApi(id, parentApi, skill, _statusFacade);
        }
    }
}
