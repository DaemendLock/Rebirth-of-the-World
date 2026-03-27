using Combat.Local.Domain.Facades;

namespace Combat.API.Adapters
{
    public readonly struct SceneApiAdapter
    {
        private readonly SceneFacade _sceneFacade;
        private readonly CharacterApiAdapter _characterApiAdapter;

        public SceneApiAdapter(SceneFacade sceneFacade, CharacterApiAdapter characterApiAdapter)
        {
            _sceneFacade = sceneFacade;
            _characterApiAdapter = characterApiAdapter;
        }

        public SceneApi Get() => new(_sceneFacade, _characterApiAdapter);
    }
}
