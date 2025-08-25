using UnityEngine;

namespace Combat.Local.Presentation.Units.ViewModels
{
    public class Action
    {
        public Action(AnimationClip clip, float activeTime)
        {
            Clip = clip;
            ActiveTime = activeTime;
        }

        public AnimationClip Clip { get; }

        public float ActiveTime { get; set; }
    }
}
