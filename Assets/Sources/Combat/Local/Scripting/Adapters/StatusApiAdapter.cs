using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;
using Combat.Local.Domain.Repositories;
using Combat.Local.Scripting.Contexts;

namespace Combat.API.Adapters
{
    public sealed class StatusApiAdapter : IStatusApiAdapter
    {
        private readonly ICharacterApiAdapter _unitApiProvider;
        private readonly IAbilityApiAdapter _skillApiProvider;
        private readonly ISceneApiAdapter _sceneApiProvider;
        private readonly StatusFacade _statusFacade;

        public StatusApiAdapter(ICharacterApiAdapter unitApiRepository, IAbilityApiAdapter skillApiRepository, StatusFacade statusController, ISceneApiAdapter sceneApiProvider)
        {
            _unitApiProvider = unitApiRepository;
            _skillApiProvider = skillApiRepository;
            _sceneApiProvider = sceneApiProvider;
            _statusFacade = statusController;
        }

        public StatusApi Adaptee(StatusId id, UnitId parent, AbilityKey? abilityKey)
        {
            Unit parentApi = _unitApiProvider.Adaptee(parent);

            AbilityApi skill = null;

            if (abilityKey.HasValue)
            {
                skill = _skillApiProvider.Adaptee(abilityKey.Value);
            }

            return new(new StatusContext(id, _statusFacade), parentApi, skill);
        }
    }
}
