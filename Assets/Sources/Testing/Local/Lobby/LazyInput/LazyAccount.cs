using Combat.Common.Primitives;

using Lobby.Common.Primitives;
using Lobby.Local.Application.UseCases.Scenarios;
using Lobby.Local.Domain.UseCases.Accounts;

using UnityEngine;

namespace Assets.Sources.Testing.Local.Lobby.LazyInput
{
    [DefaultExecutionOrder(-1000)]
    public sealed class LazyAccount : MonoBehaviour
    {
        [Zenject.Inject] private readonly AccountCreateUseCase _accountCreateUseCase;
        [Zenject.Inject] private readonly ScenarioRequestJoinUseCase _scenarioRequestJoinUseCase;

        [SerializeField] private string _name = "Player";
        [SerializeField] private string _title = "Adventurer";
        [SerializeField, Min(1)] private int _level = 1;
        [SerializeField] private string _avatarCharacterId;

        [field: SerializeField] public long Guid { get; private set; }

        public AccountId AccountId => new(Guid);

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

        public void JoinScenario(ScenarioId scenarioId) => _scenarioRequestJoinUseCase.Execute(scenarioId);
    }
}
