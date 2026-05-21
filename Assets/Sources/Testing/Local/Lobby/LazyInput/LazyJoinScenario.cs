using System;

using UnityEngine;

namespace Assets.Sources.Testing.Local.Lobby.LazyInput
{
    [RequireComponent(typeof(LazyAccount))]
    public sealed class LazyJoinScenario : MonoBehaviour
    {
        [field: SerializeField] private string _scenarioId;

        private LazyAccount _lazyAccount;

        private void Awake()
        {
            _lazyAccount = GetComponent<LazyAccount>();
            enabled = false;
        }

        private void Start()
        {
            Guid guid = Guid.Parse(_scenarioId);
            _lazyAccount.JoinScenario(new(guid));
            Destroy(this);
        }
    }
}
