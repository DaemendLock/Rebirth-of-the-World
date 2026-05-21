using UnityEngine;

namespace Combat.Local.Gateways.Models
{
    public class ActivityModel
    {
        public ActivityModel(AnimationClip clip, float startTime, float recoveryTime)
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
