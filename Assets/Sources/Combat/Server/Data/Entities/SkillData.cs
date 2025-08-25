using System;

using CastStateSkill;

using Server.Combat.Domain.Skills;

using UnityEngine;

namespace Server.Combat.Data.Entities
{
    [CreateAssetMenu(menuName = "Assets/Skills/Skill")]
    public class SkillData : ScriptableObject
    {
        [field: SerializeField] public int Id { get; private set; }
        [field: SerializeField] public float Cooldown { get; private set; }
        [field: SerializeField] public SkillFlags Flags { get; private set; }
        [SerializeField, Range(0, 1)] private float[] _frameData;
        [field: SerializeField] public AnimationClip AnimationClip { get; private set; }
        [field: SerializeField] public string ScriptName { get; private set; }

        public IFrameData FrameData => new SkillFrameData(AnimationClip.length, _frameData);

#if UNITY_EDITOR
        private void OnValidate()
        {
            Array.Sort(_frameData ?? Array.Empty<float>());
        }
#endif
    }
}
