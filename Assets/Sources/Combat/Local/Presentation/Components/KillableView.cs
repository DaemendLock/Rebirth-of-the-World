using DaeAnimator;

using UnityEngine;

namespace Combat.Local.Presentation.Components
{
    [RequireComponent(typeof(CharacterAnimator))]
    public class KillableView : MonoBehaviour// BindableViewComponent<UnitViewModel>
    {
        private const string AnimatorAliveName = "Alive";

        private CharacterAnimator _animator;

        private void Awake()
        {
            _animator = GetComponent<CharacterAnimator>();
        }

        public void Kill()
        {
            _animator.SetConsciousState(false);
        }

        public void Revive()
        {
            _animator.SetConsciousState(true);
        }
    }
}
