using Combat.Local.Domain.Facades;

namespace Combat.API.Controllers
{
    public class SceneApiAdapter
    {
        private readonly SceneFacade _sceneFacade;

        public SceneApiAdapter(SceneFacade sceneFacade)
        {
            _sceneFacade = sceneFacade;
        }

        public SceneApi Get() => new(_sceneFacade);
    }
}
