using UnityEngine;

namespace DaeAnimator
{
    public readonly ref struct CharacterAnimation
    {
        public readonly AnimationClip Clip;
        public readonly float FadeIn;
        public readonly float FadeOut;
        public readonly bool IsLooped;

        public CharacterAnimation(AnimationClip clip, float fadeIn, float fadeOut, bool isLooped)
        {
            Clip = clip;
            FadeIn = fadeIn;
            FadeOut = fadeOut;
            IsLooped = isLooped;
        }
    }
}
