using CastStateSkill;

using Combat.Common.ValueObjects;

using System;

using UnityEngine;

namespace Data.Entities
{
    public class NotScriptableActionData
    {
        public NotScriptableActionData(ActionId id, AnimationClip animation, IFrameData frameData)
        {
            Id = id;
            Animation = animation;
            FrameData = frameData;
        }

        public ActionId Id { get; }

        public IFrameData FrameData { get; }

        public AnimationClip Animation { get; }
    }

    [CreateAssetMenu(menuName = "Assets/Skills/Action")]
    public class ActionData : ScriptableObject, IActionData
    {
        [SerializeField] private int _id;
        [SerializeField, Range(0, 1)] private float[] _frameData;

        public ActionId Id => new(_id);

        public IFrameData FrameData => new FrameData(Animation == null ? 0 : Animation.length, _frameData);

        [field: SerializeField] public AnimationClip Animation { get; private set; }

#if UNITY_EDITOR
        private void OnValidate()
        {
            Array.Sort(_frameData ?? Array.Empty<float>());
        }
#endif
    }

    public interface IActionData
    {
        ActionId Id { get; }
        AnimationClip Animation { get; }
        IFrameData FrameData { get; }
    }
}
