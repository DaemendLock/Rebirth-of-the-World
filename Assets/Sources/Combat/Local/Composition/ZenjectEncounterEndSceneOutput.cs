using Combat.Common.ValueObjects;
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

        public void Present(EncounterState reason)
        {
            if (_isLoading)
            {
                return;
            }

            _isLoading = true;
            _sceneLoader.LoadSceneAsync(
                LobbySceneName,
                LoadSceneMode.Single,
                container => container.BindInstance(reason).AsSingle());
        }
    }
}
