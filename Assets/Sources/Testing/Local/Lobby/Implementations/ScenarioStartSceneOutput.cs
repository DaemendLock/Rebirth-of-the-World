using Global.Local.DTO;

using Lobby.Local.Domain.Outputs;

using UnityEngine.SceneManagement;

using Zenject;

namespace Testing.Local.Lobby
{
    public sealed class ScenarioStartSceneOutput : IScenarioStartOutput
    {
        private const string CombatSceneName = "Combat";

        private readonly ZenjectSceneLoader _sceneLoader;
        private bool _isLoading;

        public ScenarioStartSceneOutput(ZenjectSceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Present(StartCombatRequest request)
        {
            if (_isLoading)
            {
                return;
            }

            _isLoading = true;
            _sceneLoader.LoadSceneAsync(
                CombatSceneName,
                LoadSceneMode.Single,
                container => container.BindInstance(request).AsSingle());
        }
    }
}
