using System;

using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace DaeAnimator
{
    public class DeathPlayableBehaviour : PlayableBehaviour
    {
        private float _nextTime;

        private Playable _mixer;

        public void Init(CharacterAnimationPack animationPack, Playable owner, int ownerOutputPort, PlayableGraph graph)
        {
            owner.SetInputCount(1);
            _mixer = AnimationMixerPlayable.Create(graph, 3);
            graph.Connect(_mixer, 0, owner, 0);

            graph.Connect(AnimationClipPlayable.Create(graph, animationPack.DeathAnimation), 0, _mixer, 0);
            graph.Connect(AnimationClipPlayable.Create(graph, animationPack.DeadAnimation), 0, _mixer, 1);
            graph.Connect(AnimationClipPlayable.Create(graph, animationPack.ReviveAnimation), 0, _mixer, 2);

        }
    }

    [RequireComponent(typeof(Animator))]
    public class CharacterAnimator : MonoBehaviour
    {
        [SerializeField] private CharacterAnimationPack _animationPack;

        private PlayableGraph _grapth;
        private PlayableOutput _playableOutput;
        private AnimationMixerPlayable _consciousStateMixer;

        private Playable _alivePlayable;
        private Playable _deadPlayable;

        private AnimationClipPlayable _deathClip;

        private AnimationClipPlayable _cast;

        private void Start()
        {
            _grapth = PlayableGraph.Create(gameObject.name);
            _grapth.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            _playableOutput = AnimationPlayableOutput.Create(_grapth, "Character animator output", GetComponent<Animator>());
            _consciousStateMixer = AnimationMixerPlayable.Create(_grapth, 2);
            _playableOutput.SetSourcePlayable(_consciousStateMixer);
            CreateAlive();
            CreateDead();

            _consciousStateMixer.SetInputWeight(0, 1f);

            _grapth.Play();
        }

        private void OnDestroy()
        {
            if (_grapth.IsValid() == false)
            {
                return;
            }

            _grapth.Destroy();
        }

        public void SetTimeScale()
        {

        }

        public void PlayCastAnimation(CharacterAnimation animation, float clipTime = 0)
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

                _cast = AnimationClipPlayable.Create(_grapth, animation.Clip);
                _cast.SetApplyFootIK(true);
                _grapth.Connect(_cast, 0, _alivePlayable, 1);
                _alivePlayable.SetInputWeight(0, 0f);
                _alivePlayable.SetInputWeight(1, 1f);
            }

            if (_cast.IsValid())
            {
                _cast.SetTime(clipTime);
            }
        }

        public void StopCastAnimation()
        {
            _alivePlayable.SetInputWeight(0, 1f);
            _alivePlayable.SetInputWeight(1, 0f);

            if (_cast.IsValid())
            {
                _cast.Destroy();
            }
        }

        public void SetConsciousState(bool alive)
        {
            if (alive)
            {
                Resurrect();
            }
            else
            {
                Kill();
            }
        }

        private void CreateAlive()
        {
            _alivePlayable = AnimationMixerPlayable.Create(_grapth, 2);
            _grapth.Connect(AnimationClipPlayable.Create(_grapth, _animationPack.IdleAnimation), 0, _alivePlayable, 0);
            _alivePlayable.SetInputWeight(0, 1f);
            _grapth.Connect(_alivePlayable, 0, _consciousStateMixer, 0);
        }

        private void CreateDead()
        {
            _deadPlayable = AnimationMixerPlayable.Create(_grapth, 1);
            _deadPlayable.SetInputWeight(0, 1f);
            _grapth.Connect(_deadPlayable, 0, _consciousStateMixer, 1);
        }

        private void Kill()
        {
            _consciousStateMixer.SetInputWeight(0, 0);
            _consciousStateMixer.SetInputWeight(1, 1f);
            _alivePlayable.Pause();
            //_grapth.Connect(AnimationClipPlayable.Create(_grapth, _animationPack.DeathAnimation), 0, _deadPlayable, 0);
            _deadPlayable.Play();
        }

        private void Resurrect()
        {
            _consciousStateMixer.SetInputWeight(0, 1f);
            _consciousStateMixer.SetInputWeight(1, 0f);
            _deadPlayable.Pause();
            _alivePlayable.Play();
        }
    }

    public interface ICharacterAnimator
    {
        void SetConsciousState(bool alive);

        void SetSpeedVector(Vector2 value);

        void StartCast(CharacterAnimation animation);
        void StopCast();
        void InterruptCast();
    }

    public class ScriptAnimator
    {
        private readonly string _name;
        private readonly Animator _animator;

        private PlayableGraph _graph;

        private PlayableOutput _playableOutput;

        public ScriptAnimator(string name, Animator animator)
        {
            _name = name;
            _animator = animator;
        }

        public void Create()
        {
            _graph = PlayableGraph.Create(_name);

            _playableOutput = AnimationPlayableOutput.Create(_graph, _name + " Animation Output", _animator);
        }

        public void Destroy()
        {
            _graph.Destroy();
        }
    }

    [Serializable]
    public class CharacterAnimationPack
    {
        public AnimationClip IdleAnimation;
        public readonly AnimationClip RunForwardAnimation;
        public readonly AnimationClip RunBackwardAnimation;
        public readonly AnimationClip StrafeAnimation;

        public AnimationClip DeathAnimation;
        public readonly AnimationClip DeadAnimation;
        public readonly AnimationClip ReviveAnimation;
    }
}