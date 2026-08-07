using UnityEngine;

namespace DaeAnimator
{
    public readonly ref struct CharacterAnimation
    {
        public readonly AnimationClip Clip;
        public readonly float FadeIn;
        public readonly float FadeOut;
        public readonly bool IsLooped;
        public readonly float LoopStartTime;
        public readonly float LoopEndTime;

        public CharacterAnimation(AnimationClip clip, float fadeIn, float fadeOut, bool isLooped)
            : this(clip, fadeIn, fadeOut, isLooped, 0f, clip == null ? 0f : clip.length)
        {
        }

        public CharacterAnimation(
            AnimationClip clip,
            float fadeIn,
            float fadeOut,
            bool isLooped,
            float loopStartTime,
            float loopEndTime)
        {
            Clip = clip;
            FadeIn = fadeIn;
            FadeOut = fadeOut;
            IsLooped = isLooped;
            LoopStartTime = loopStartTime;
            LoopEndTime = loopEndTime;
        }
    }
}
