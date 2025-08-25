using UnityEngine;

namespace DaeAnimator
{
    public struct UnitAnimation
    {
        public readonly AnimationClip Clip;
        public readonly float FadeIn;
        public readonly float FadeOut;

        public UnitAnimation(AnimationClip clip, float fadeIn, float fadeOut)
        {
            Clip = clip;
            FadeIn = fadeIn;
            FadeOut = fadeOut;
        }
    }
}
