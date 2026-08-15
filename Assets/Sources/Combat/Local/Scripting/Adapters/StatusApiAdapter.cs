using Combat.API;
using Combat.API.Adapters;
using Combat.Common.ValueObjects;
using Combat.Local.Domain.Facades;
using Combat.Local.Scripting.Contexts;

namespace Combat.Local.Scripting.Adapters
{
    public sealed class StatusApiAdapter : IStatusApiAdapter
    {
        private readonly CharacterApiAdapter _unitApiProvider;
        private readonly AbilityApiAdapter _skillApiProvider;
        private readonly ISceneApiAdapter _sceneApiProvider;
        private readonly StatusFacade _statusFacade;

        public StatusApiAdapter(CharacterApiAdapter unitApiRepository, AbilityApiAdapter skillApiRepository, StatusFacade statusController, ISceneApiAdapter sceneApiProvider)
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
