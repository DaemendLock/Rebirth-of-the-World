using Lobby.Common.Primitives;
using Lobby.Local.Domain.UseCases.Accounts;
using Lobby.Local.Domain.Entities;
using Lobby.Local.Domain.UseCases.Scenarios;

using UnityEngine;

namespace Assets.Sources.Testing.Local.Lobby.LazyInput
{
    [DefaultExecutionOrder(-1000)]
    public sealed class LazyAccount : MonoBehaviour
    {
        [Zenject.Inject] private AccountCreateUseCase _accountCreateUseCase;
        [Zenject.Inject] private ScenarioJoinUseCase _scenarioJoinUseCase;
        [Zenject.Inject] private LobbySession _lobbySession;

        [SerializeField] private string _name = "Player";
        [SerializeField] private string _title = "Adventurer";
        [SerializeField, Min(1)] private int _level = 1;
        [SerializeField] private string _avatarCharacterId;

        [field: SerializeField] public long Guid { get; private set; }

        public AccountId AccountId => _lobbySession.ActiveAccountId;

        private void Start()
        {
            CharacterKey? avatarCharacterId = string.IsNullOrWhiteSpace(_avatarCharacterId)
                ? null
                : new CharacterKey(_avatarCharacterId);

            Guid = _accountCreateUseCase.Execute(
                _name,
                _title,
                _level,
                avatarCharacterId).Value;
        }

        public void JoinScenario(ScenarioId scenarioId)
        {
            _scenarioJoinUseCase.Execute(scenarioId);
        }
    }
}
