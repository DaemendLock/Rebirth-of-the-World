using CastStateSkill;

using Combat.Common.Primitives;

using UnityEngine;

namespace Combat.Local.Presentation.Providers
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

    public interface IActionDataContainer
    {
        ActionData Get(ActionId id);
    }
}
