using UnityEngine;

namespace DaeAnimator
{
    public struct CharacterAnimation
    {
        public readonly AnimationClip Clip;
        public readonly float FadeIn;
        public readonly float FadeOut;

        public CharacterAnimation(AnimationClip clip, float fadeIn, float fadeOut)
        {
            Clip = clip;
            FadeIn = fadeIn;
            FadeOut = fadeOut;
        }
    }
}
