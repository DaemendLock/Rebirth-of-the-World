using UnityEngine;

namespace View.Combat.Units.Animations
{
    [RequireComponent(typeof(Animator))]
    public class AnimationController : MonoBehaviour
    {

        [SerializeField] private UnityEngine.Animation _idleAnimation;
        [SerializeField] private UnityEngine.Animation _walkAnimation;
        [SerializeField] private UnityEngine.Animation _deathAnimation;

        [SerializeField] private Animator _animator;

        private void Start()
        {
            _animator = GetComponent<Animator>();
        }

        public void PlayAttackAnimation()
        {

        }
    }
}
