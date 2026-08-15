using System;

using UnityEngine;
using UnityEngine.Animations;
using UnityEngine.Playables;

namespace DaeAnimator
{
    [RequireComponent(typeof(Animator))]
    public sealed class CharacterAnimator : MonoBehaviour, ICharacterAnimator
    {
        private const int AliveInput = 0;
        private const int DeadInput = 1;

        private const int LocomotionInput = 0;
        private const int CastInput = 1;

        private const int IdleInput = 0;
        private const int ForwardInput = 1;
        private const int BackwardInput = 2;
        private const int StrafeInput = 3;
        private const int LocomotionInputCount = 4;

        private enum LifeState
        {
            Alive,
            Dying,
            Dead,
            Reviving
        }

        [SerializeField] private CharacterAnimationPack _animationPack;

        private readonly bool[] _hasLocomotionClip = new bool[LocomotionInputCount];
        private readonly float[] _locomotionWeights = new float[LocomotionInputCount];

        private PlayableGraph _graph;
        private AnimationPlayableOutput _playableOutput;
        private AnimationMixerPlayable _consciousStateMixer;
        private AnimationMixerPlayable _aliveMixer;
        private AnimationMixerPlayable _locomotionMixer;
        private AnimationMixerPlayable _deadMixer;

        private AnimationClipPlayable _castPlayable;
        private AnimationClipPlayable _lifePlayable;

        private LifeState _lifeState = LifeState.Alive;
        private Vector2 _speedVector;

        private float _timeScale = 1f;
        private float _castWeight;
        private float _castFadeIn;
        private float _castFadeOut;
        private float _castLoopStartTime;
        private float _castLoopEndTime;
        private bool _castIsStopping;
        private bool _castIsLooped;

        private bool _lifeClipIsLooped;

        private void Awake()
        {
            CreateGraph();
        }

        private void OnEnable()
        {
            if (!_graph.IsValid())
            {
                return;
            }

            _graph.Play();
        }

        private void OnDisable()
        {
            if (!_graph.IsValid())
            {
                return;
            }

            _graph.Stop();
        }

        private void Update()
        {
            float deltaTime = Time.deltaTime * _timeScale;

            UpdateCast(deltaTime);
            UpdateLifeState();
        }

        private void OnDestroy()
        {
            if (!_graph.IsValid())
            {
                return;
            }

            _graph.Destroy();
        }

        public void SetTimeScale(float value)
        {
            _timeScale = Mathf.Max(0f, value);

            if (_consciousStateMixer.IsValid())
            {
                _consciousStateMixer.SetSpeed(_timeScale);
            }
        }

        public void SetSpeedVector(Vector2 value)
        {
            _speedVector = Vector2.ClampMagnitude(value, 1f);
            ApplyLocomotionWeights();
        }

        public void StartCast(CharacterAnimation animation)
        {
            PlayCastAnimation(animation);
        }

        public void StopCast()
        {
            StopCastAnimation();
        }

        public void ReleaseCast()
        {
            ReleaseCastAnimation();
        }

        public void InterruptCast()
        {
            DestroyCastPlayable();
        }

        public void PlayCastAnimation(CharacterAnimation animation, float clipTime = 0f)
        {
            if (animation.Clip == null || _lifeState != LifeState.Alive)
            {
                return;
            }

            DestroyCastPlayable();

            _castPlayable = AnimationClipPlayable.Create(_graph, animation.Clip);
            _castPlayable.SetApplyFootIK(true);

            _castLoopStartTime = Mathf.Clamp(animation.LoopStartTime, 0f, animation.Clip.length);
            _castLoopEndTime = Mathf.Clamp(animation.LoopEndTime, _castLoopStartTime, animation.Clip.length);
            _castIsLooped = animation.IsLooped;

            _castPlayable.SetTime(NormalizeClipTime(
                animation.Clip,
                clipTime,
                _castIsLooped,
                _castLoopStartTime,
                _castLoopEndTime));

            _graph.Connect(_castPlayable, 0, _aliveMixer, CastInput);

            _castFadeIn = Mathf.Max(0f, animation.FadeIn);
            _castFadeOut = Mathf.Max(0f, animation.FadeOut);
            _castIsStopping = false;
            _castWeight = _castFadeIn > 0f ? 0f : 1f;

            SetCastWeight(_castWeight);
            _castPlayable.Play();
        }

        public void ReleaseCastAnimation()
        {
            if (_castPlayable.IsValid() == false || _castIsLooped == false)
            {
                return;
            }

            _castIsLooped = false;
            _castPlayable.SetTime(_castLoopEndTime);
        }

        public void StopCastAnimation()
        {
            if (_castPlayable.IsValid() == false)
            {
                return;
            }

            if (_castFadeOut <= 0f)
            {
                DestroyCastPlayable();
                return;
            }

            _castIsStopping = true;
        }

        public void SetConsciousState(bool alive)
        {
            if (alive)
            {
                BeginRevive();
            }
            else
            {
                BeginDeath();
            }
        }

        private void CreateGraph()
        {
            _graph = PlayableGraph.Create($"{gameObject.name} Character Animator");
            _graph.SetTimeUpdateMode(DirectorUpdateMode.GameTime);

            _playableOutput = AnimationPlayableOutput.Create(_graph, "Character animator output", GetComponent<Animator>());
            _consciousStateMixer = AnimationMixerPlayable.Create(_graph, 2);
            _aliveMixer = AnimationMixerPlayable.Create(_graph, 2);
            _locomotionMixer = AnimationMixerPlayable.Create(_graph, LocomotionInputCount);
            _deadMixer = AnimationMixerPlayable.Create(_graph, 1);

            _graph.Connect(_locomotionMixer, 0, _aliveMixer, LocomotionInput);
            _graph.Connect(_aliveMixer, 0, _consciousStateMixer, AliveInput);
            _graph.Connect(_deadMixer, 0, _consciousStateMixer, DeadInput);

            _aliveMixer.SetInputWeight(LocomotionInput, 1f);
            _deadMixer.SetInputWeight(0, 1f);
            SetConsciousMixerWeights(alive: true);

            CreateLocomotionClip(IdleInput, _animationPack?.IdleAnimation);
            CreateLocomotionClip(ForwardInput, _animationPack?.RunForwardAnimation);
            CreateLocomotionClip(BackwardInput, _animationPack?.RunBackwardAnimation);
            CreateLocomotionClip(StrafeInput, _animationPack?.StrafeAnimation);
            ApplyLocomotionWeights();

            _playableOutput.SetSourcePlayable(_consciousStateMixer);
            _graph.Play();
        }

        private void CreateLocomotionClip(int input, AnimationClip clip)
        {
            if (clip == null)
            {
                return;
            }

            AnimationClipPlayable playable = AnimationClipPlayable.Create(_graph, clip);
            playable.SetApplyFootIK(true);
            _graph.Connect(playable, 0, _locomotionMixer, input);
            _hasLocomotionClip[input] = true;
        }

        private void ApplyLocomotionWeights()
        {
            if (_locomotionMixer.IsValid() == false)
            {
                return;
            }

            Array.Clear(_locomotionWeights, 0, _locomotionWeights.Length);

            float movementWeight = Mathf.Clamp01(_speedVector.magnitude);
            float directionTotal = Mathf.Abs(_speedVector.x) + Mathf.Abs(_speedVector.y);

            _locomotionWeights[IdleInput] = 1f - movementWeight;

            if (directionTotal > Mathf.Epsilon)
            {
                float directionScale = movementWeight / directionTotal;
                _locomotionWeights[ForwardInput] = Mathf.Max(0f, _speedVector.y) * directionScale;
                _locomotionWeights[BackwardInput] = Mathf.Max(0f, -_speedVector.y) * directionScale;
                _locomotionWeights[StrafeInput] = Mathf.Abs(_speedVector.x) * directionScale;
            }

            MoveMissingClipWeightsToIdle();
            NormalizeLocomotionWeights();

            for (int i = 0; i < _locomotionWeights.Length; i++)
            {
                _locomotionMixer.SetInputWeight(i, _locomotionWeights[i]);
            }
        }

        private void MoveMissingClipWeightsToIdle()
        {
            for (int i = 1; i < _locomotionWeights.Length; i++)
            {
                if (_hasLocomotionClip[i])
                {
                    continue;
                }

                _locomotionWeights[IdleInput] += _locomotionWeights[i];
                _locomotionWeights[i] = 0f;
            }

            if (_hasLocomotionClip[IdleInput] == false)
            {
                _locomotionWeights[IdleInput] = 0f;
            }
        }

        private void NormalizeLocomotionWeights()
        {
            float total = 0f;

            for (int i = 0; i < _locomotionWeights.Length; i++)
            {
                total += _locomotionWeights[i];
            }

            if (total <= Mathf.Epsilon)
            {
                for (int i = 0; i < _hasLocomotionClip.Length; i++)
                {
                    if (_hasLocomotionClip[i] == false)
                    {
                        continue;
                    }

                    _locomotionWeights[i] = 1f;
                    return;
                }

                return;
            }

            for (int i = 0; i < _locomotionWeights.Length; i++)
            {
                _locomotionWeights[i] /= total;
            }
        }

        private void UpdateCast(float deltaTime)
        {
            if (_castPlayable.IsValid() == false)
            {
                return;
            }

            if (_castIsLooped)
            {
                LoopPlayable(_castPlayable, _castLoopStartTime, _castLoopEndTime);
            }

            float targetWeight = _castIsStopping ? 0f : 1f;
            float fadeDuration = _castIsStopping ? _castFadeOut : _castFadeIn;

            _castWeight = fadeDuration <= 0f
                ? targetWeight
                : Mathf.MoveTowards(_castWeight, targetWeight, deltaTime / fadeDuration);

            SetCastWeight(_castWeight);

            if (_castIsStopping && _castWeight <= 0f)
            {
                DestroyCastPlayable();
            }
        }

        private void SetCastWeight(float weight)
        {
            _aliveMixer.SetInputWeight(LocomotionInput, 1f - weight);
            _aliveMixer.SetInputWeight(CastInput, weight);
        }

        private void DestroyCastPlayable()
        {
            if (_castPlayable.IsValid())
            {
                _aliveMixer.DisconnectInput(CastInput);
                _castPlayable.Destroy();
            }

            _castWeight = 0f;
            _castLoopStartTime = 0f;
            _castLoopEndTime = 0f;
            _castIsStopping = false;
            _castIsLooped = false;

            if (_aliveMixer.IsValid())
            {
                SetCastWeight(0f);
            }
        }

        private void BeginDeath()
        {
            if (_lifeState == LifeState.Dying || _lifeState == LifeState.Dead)
            {
                return;
            }

            InterruptCast();
            _aliveMixer.SetSpeed(0d);

            if (_animationPack?.DeathAnimation != null)
            {
                PlayLifeClip(_animationPack.DeathAnimation, isLooped: false);
                SetConsciousMixerWeights(alive: false);
                _lifeState = LifeState.Dying;
                return;
            }

            EnterDeadState();
        }

        private void EnterDeadState()
        {
            _lifeState = LifeState.Dead;

            if (_animationPack?.DeadAnimation != null)
            {
                PlayLifeClip(_animationPack.DeadAnimation, isLooped: true);
                SetConsciousMixerWeights(alive: false);
                return;
            }

            if (_lifePlayable.IsValid())
            {
                _lifePlayable.SetTime(_lifePlayable.GetAnimationClip().length);
                _lifePlayable.SetSpeed(0d);
                SetConsciousMixerWeights(alive: false);
                return;
            }

            // With no death assets, preserve the last alive pose instead of outputting a T-pose.
            SetConsciousMixerWeights(alive: true);
        }

        private void BeginRevive()
        {
            if (_lifeState == LifeState.Alive || _lifeState == LifeState.Reviving)
            {
                return;
            }

            if (_animationPack?.ReviveAnimation == null)
            {
                CompleteRevive();
                return;
            }

            PlayLifeClip(_animationPack.ReviveAnimation, isLooped: false);
            SetConsciousMixerWeights(alive: false);
            _lifeState = LifeState.Reviving;
        }

        private void CompleteRevive()
        {
            DestroyLifePlayable();
            _aliveMixer.SetSpeed(1d);
            SetConsciousMixerWeights(alive: true);
            _lifeState = LifeState.Alive;
            ApplyLocomotionWeights();
        }

        private void PlayLifeClip(AnimationClip clip, bool isLooped)
        {
            DestroyLifePlayable();

            _lifePlayable = AnimationClipPlayable.Create(_graph, clip);
            _lifePlayable.SetApplyFootIK(true);
            _lifePlayable.SetTime(0d);
            _graph.Connect(_lifePlayable, 0, _deadMixer, 0);
            _deadMixer.SetInputWeight(0, 1f);
            _lifeClipIsLooped = isLooped;
            _lifePlayable.Play();
        }

        private void UpdateLifeState()
        {
            if (_lifePlayable.IsValid() == false)
            {
                return;
            }

            if (_lifeClipIsLooped)
            {
                LoopPlayable(_lifePlayable);
                return;
            }

            AnimationClip clip = _lifePlayable.GetAnimationClip();

            if (_lifePlayable.GetTime() < clip.length)
            {
                return;
            }

            switch (_lifeState)
            {
                case LifeState.Dying:
                    EnterDeadState();
                    break;
                case LifeState.Reviving:
                    CompleteRevive();
                    break;
            }
        }

        private void DestroyLifePlayable()
        {
            if (_lifePlayable.IsValid())
            {
                _deadMixer.DisconnectInput(0);
                _lifePlayable.Destroy();
            }

            _lifeClipIsLooped = false;
        }

        private void SetConsciousMixerWeights(bool alive)
        {
            _consciousStateMixer.SetInputWeight(AliveInput, alive ? 1f : 0f);
            _consciousStateMixer.SetInputWeight(DeadInput, alive ? 0f : 1f);
        }

        private static double NormalizeClipTime(
            AnimationClip clip,
            float clipTime,
            bool isLooped,
            float loopStartTime,
            float loopEndTime)
        {
            double time = Math.Max(0d, clipTime);

            if (clip.length <= Mathf.Epsilon)
            {
                return 0d;
            }

            if (isLooped && time >= loopEndTime)
            {
                double loopDuration = loopEndTime - loopStartTime;

                return loopDuration > Mathf.Epsilon
                    ? loopStartTime + (time - loopStartTime) % loopDuration
                    : loopEndTime;
            }

            return Math.Min(time, clip.length);
        }

        private static void LoopPlayable(AnimationClipPlayable playable)
        {
            AnimationClip clip = playable.GetAnimationClip();

            if (clip == null || clip.length <= Mathf.Epsilon)
            {
                return;
            }

            double time = playable.GetTime();

            if (time >= clip.length)
            {
                playable.SetTime(time % clip.length);
            }
        }

        private static void LoopPlayable(AnimationClipPlayable playable, float loopStartTime, float loopEndTime)
        {
            double time = playable.GetTime();

            if (time < loopEndTime)
            {
                return;
            }

            double loopDuration = loopEndTime - loopStartTime;
            double loopTime = loopDuration > Mathf.Epsilon
                ? loopStartTime + (time - loopStartTime) % loopDuration
                : loopEndTime;

            playable.SetTime(loopTime);
        }
    }

    public interface ICharacterAnimator
    {
        void SetConsciousState(bool alive);

        void SetSpeedVector(Vector2 value);

        void StartCast(CharacterAnimation animation);
        void ReleaseCast();
        void StopCast();
        void InterruptCast();
    }

    [Serializable]
    public sealed class CharacterAnimationPack
    {
        public AnimationClip IdleAnimation;
        public AnimationClip RunForwardAnimation;
        public AnimationClip RunBackwardAnimation;
        public AnimationClip StrafeAnimation;

        public AnimationClip DeathAnimation;
        public AnimationClip DeadAnimation;
        public AnimationClip ReviveAnimation;
    }
}
