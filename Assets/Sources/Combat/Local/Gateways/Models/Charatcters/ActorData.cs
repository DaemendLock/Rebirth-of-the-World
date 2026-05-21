using Combat.Common.ValueObjects;
using Combat.Local.Domain.Entities;
using Combat.Local.Domain.ValueObjects;
using Combat.Local.Gateways.DataSources;

using DaeAnimator;

using UnityEngine;

namespace Combat.Local.Gateways.Models
{
    public readonly struct ActorData
    {
        public ActorData(ActorState state, Action action, ConsciousState consciousState, DesiredActions desiredActions)
        {
            State = state;
            Action = action;
            ConsciousState = consciousState;
            DesiredActions = desiredActions;
        }

        public ActorState State { get; }

        public Action Action { get; }

        public ConsciousState ConsciousState { get; }

        public DesiredActions DesiredActions { get; }
    }

    public sealed class ActorModelComponent : MonoBehaviour
    {
        private CharacterAnimator _characterAnimator;

        private ConsciousState _consciousState;

        private void Awake()
        {
            _characterAnimator = GetComponent<CharacterAnimator>();
        }

        public ActorState State { get; set; }

        public Action Action { get; set; }

        public DesiredActions DesiredActions { get; set; }

        public ConsciousState ConsciousState
        {
            get => _consciousState;
            set
            {
                _consciousState = value;

                switch (value)
                {
                    case ConsciousState.Alive:
                        _characterAnimator.SetConsciousState(true);
                        return;
                    case ConsciousState.Dead:
                        _characterAnimator.SetConsciousState(false);
                        return;
                    default:
                        return;
                }
            }
        }
    }
}
