using Lobby.Common.Primitives;
using Lobby.Local.Domain.UseCases.Accounts;
using Lobby.Local.Domain.UseCases.Scenarios;

using UnityEngine;

namespace Assets.Sources.Testing.Local.Lobby.LazyInput
{
    public sealed class LazyAccount : MonoBehaviour
    {
        [Zenject.Inject] private AccountCreateUseCase _accountCreateUseCase;
        [Zenject.Inject] private ScenarioJoinUseCase _scenarioJoinUseCase;

        [field: SerializeField] public long Guid { get; private set; }

        public AccountId AccountId => new(Guid);

        private void Start()
        {
            Guid = _accountCreateUseCase.Execute().Value;


        }

        public void JoinScenario(ScenarioId scenarioId)
        {
            _scenarioJoinUseCase.Execute(scenarioId, AccountId);
        }
    }
}
