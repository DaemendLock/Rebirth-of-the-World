namespace Combat.API.Controllers
{
    public class SceneApiProvider
    {
        private SceneApi _scene;

        public void Register(SceneApi value) => _scene ??= value;

        public SceneApi Get() => _scene;
    }
}
