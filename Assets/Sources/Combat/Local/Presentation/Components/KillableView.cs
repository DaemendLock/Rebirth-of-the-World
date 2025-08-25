using Combat.Local.Presentation.Units.ViewModels;

using UnityEngine;

namespace Client.Combat.Presentation.Implementations.Units
{
    [RequireComponent(typeof(Animator))]
    public class KillableView : BindableViewComponent<UnitViewModel>
    {
        private const string AnimatorAliveName = "Alive";

        private Animator _animator;

        private void Awake()
        {
            _animator = GetComponent<Animator>();
        }

        private void Update()
        {
            _animator.SetBool(AnimatorAliveName, Model.Alive);
        }
    }
}
