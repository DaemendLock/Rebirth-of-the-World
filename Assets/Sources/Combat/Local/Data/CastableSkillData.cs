using CastStateSkill;

using Combat.Common.Flags;
using Combat.Common.ValueObjects;

using Data.Entities;

using System;

using UnityEngine;

namespace Combat.Local.Data.Entities
{
    [CreateAssetMenu(menuName = "Assets/Skill/CastableSkill")]
    public class CastableSkillData : ScriptableObject, ISkillData
    {
        [SerializeField] private int _id;
        [field: SerializeField] public float Cooldown { get; private set; }
        [field: SerializeField] public SkillFlags Flags { get; private set; }
        [SerializeField, Range(0, 1)] private float[] _frameData;
        [field: SerializeField] public AnimationClip AnimationClip { get; private set; }
        [field: SerializeField] public string ScriptName { get; private set; }

        public SkillId Id => new(_id);

        public IFrameData FrameData => new FrameData(AnimationClip == null ? 0 : AnimationClip.length, _frameData);

#if UNITY_EDITOR
        private void OnValidate()
        {
            Array.Sort(_frameData ?? Array.Empty<float>());
        }
#endif
    }
}
