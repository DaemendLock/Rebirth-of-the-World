using CastStateSkill;

using Combat.Common.ValueObjects;
using Combat.Local.Data.Entities;
using Combat.Local.Gateways.DataSources;

using Data.Entities;

using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Data.Databases
{
    public class SkillDataBase : ISkillDataBase
    {
        private readonly Dictionary<SkillId, ISkillData> _values;
        private readonly Dictionary<SkillId, IFrameData> _frameData;
        private readonly Dictionary<SkillId, AnimationClip> _animations;

        public SkillDataBase()
        {
            _values = new();
            _frameData = new();
            _animations = new();

            foreach (CastableSkillData data in Resources.LoadAll<CastableSkillData>("Temp/TestSkills"))
            {
                Load(data);
            }
        }

        public void Load(ISkillData value)
        {
            SkillId id = value.Id;

            if (IsLoaded(id))
            {
                return;
            }

            _values[value.Id] = value;

            if (value is CastableSkillData castableSkill)
            {
                _animations.Add(id, castableSkill.AnimationClip);
            }

            if (value is CastableSkillData castable)
            {
                _frameData[value.Id] = castable.FrameData;
            }
        }

        public ISkillData Get(SkillId id) => _values[id];

        public IFrameData GetFrameData(SkillId id) => _frameData.GetValueOrDefault(id, null);

        public AnimationClip GetAnimation(SkillId id) => _animations.GetValueOrDefault(id, null);

        public void Free(SkillId id) => _values.Remove(id);

        public bool IsLoaded(SkillId id) => _values.ContainsKey(id);
    }
}
