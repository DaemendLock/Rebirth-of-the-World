using CastStateSkill;

using UnityEngine;

namespace Combat.Local.Data.Models
{
    public readonly struct ActionData
    {
        public ActionData(AnimationClip animation, IFrameData frameData)
        {
            Animation = animation;
            FrameData = frameData;
        }

        public readonly AnimationClip Animation { get; }
        public readonly IFrameData FrameData { get; }
    }
}
