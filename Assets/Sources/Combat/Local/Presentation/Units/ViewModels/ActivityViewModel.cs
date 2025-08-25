using UnityEngine;

namespace Combat.Local.Presentation.Units.ViewModels
{
    public class ActivityViewModel
    {
        public ActivityViewModel(AnimationClip clip, float startTime, float recoveryTime)
        {
            Clip = clip;
            StartTime = startTime;
            RecoveryTime = recoveryTime;
        }

        public AnimationClip Clip { get; }

        public float StartTime { get; }
        public float RecoveryTime { get; }
    }
}
