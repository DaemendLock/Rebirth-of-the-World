using Game.Application.UseCases.Scenarios;

using UnityEngine;

namespace Assets.Sources.Testing.Local.Lobby.LazyInput
{
    public sealed class LazyCreateScenario : MonoBehaviour
    {
        [Zenject.Inject] private ScenarioCreateUseCase _useCase;

        [SerializeField] private string _name;
        [SerializeField] private string _location;
        [SerializeField] private int _maxPlayers;

        private void Awake()
        {
            enabled = false;
        }

        private void OnEnable()
        {
            enabled = false;
            var id = _useCase.Execute(_name, _location, _maxPlayers);
            Debug.Log(id);
        }
    }
}
