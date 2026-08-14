using Combat.Local.Domain.OutputPorts;

using UnityEngine.SceneManagement;

using Zenject;

namespace Combat.Local.Composition
{
    public sealed class ZenjectEncounterEndSceneOutput : IEncounterEndOutput
    {
        private const string LobbySceneName = "Lobby";

        private readonly ZenjectSceneLoader _sceneLoader;
        private bool _isLoading;

        public ZenjectEncounterEndSceneOutput(ZenjectSceneLoader sceneLoader)
        {
            _sceneLoader = sceneLoader;
        }

        public void Present()
        {
            if (_isLoading)
            {
                return;
            }

            _isLoading = true;
            _sceneLoader.LoadSceneAsync(LobbySceneName, LoadSceneMode.Single);
        }
    }
}
