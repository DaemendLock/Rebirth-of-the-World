using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace DaeAnimator
{
    [RequireComponent(typeof(UnityEngine.Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        private PlayableGraph _playableGrapth;
        private PlayableOutput _playableOutput;
        private AnimationMixerPlayable _animationMixer;

        private AnimationClipPlayable _cast;

        private void Start()
        {
            _playableGrapth = PlayableGraph.Create();
            _playableGrapth.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            _playableOutput = AnimationPlayableOutput.Create(_playableGrapth, "Cast", GetComponent<UnityEngine.Animator>());
            _animationMixer = AnimationMixerPlayable.Create(_playableGrapth, 3);

            _playableGrapth.Play();
        }

        private void OnDestroy()
        {
            if (_playableGrapth.IsValid() == false)
            {
                return;
            }

            _playableGrapth.Destroy();
        }

        public void Play2(float x, float y)
        {

        }

        public void Play(CharacterAnimation animation, float clipTime = 0)
        {
            //if (animation == default)
            //{
            //    return;
            //}

            if (!_cast.IsValid() || _cast.GetAnimationClip() != animation.Clip)
            {
                if (_cast.IsValid())
                {
                    _cast.Destroy();
                }

                _cast = AnimationClipPlayable.Create(_playableGrapth, animation.Clip);
                _cast.SetApplyFootIK(true);
                _playableOutput.SetSourcePlayable(_cast);
            }

            if (_cast.IsValid())
            {
                _cast.SetTime(clipTime);
            }
        }

        public void SetRunSpeed(Vector2 runSpeed)
        {

        }
    }
}