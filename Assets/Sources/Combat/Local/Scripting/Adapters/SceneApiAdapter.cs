using Combat.Local.Domain.Facades;

namespace Combat.API.Adapters
{
    public sealed class SceneApiAdapter : ISceneApiAdapter
    {
        private readonly EncounterFacade _sceneFacade;
        private readonly ICharacterApiAdapter _characterApiAdapter;

        public SceneApiAdapter(EncounterFacade sceneFacade, ICharacterApiAdapter characterApiAdapter)
        {
            _sceneFacade = sceneFacade;
            _characterApiAdapter = characterApiAdapter;
        }

        public IEncounterApi Get() => new EncounterApi(_sceneFacade, _characterApiAdapter);
    }
}
