using Combat.Local.Domain.Facades;

namespace Combat.API.Adapters
{
    public readonly struct SceneApiAdapter
    {
        private readonly EncounterFacade _sceneFacade;
        private readonly CharacterApiAdapter _characterApiAdapter;

        public SceneApiAdapter(EncounterFacade sceneFacade, CharacterApiAdapter characterApiAdapter)
        {
            _sceneFacade = sceneFacade;
            _characterApiAdapter = characterApiAdapter;
        }

        public EncounterApi Get() => new(_sceneFacade, _characterApiAdapter);
    }
}
