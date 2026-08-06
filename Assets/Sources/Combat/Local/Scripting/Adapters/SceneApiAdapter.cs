using Combat.Local.Domain.Facades;
using Combat.Local.Scripting.Contexts;

namespace Combat.API.Adapters
{
    public sealed class SceneApiAdapter : ISceneApiAdapter
    {
        private readonly EncounterFacade _sceneFacade;
        private readonly CharacterApiAdapter _characterApiAdapter;

        public SceneApiAdapter(EncounterFacade sceneFacade, CharacterApiAdapter characterApiAdapter)
        {
            _sceneFacade = sceneFacade;
            _characterApiAdapter = characterApiAdapter;
        }

        public EncounterApi Get() => new(new DomainEncounterContext(_sceneFacade, _characterApiAdapter));
    }
}
