using CastStateSkill;

using Combat.Common.ValueObjects;
using Combat.Local.Gateways.DataSources;

using Data.Entities;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Combat.Local.Data.Databases
{
    public class SkillDataBase : ISkillDataBase
    {
        private readonly Dictionary<SkillId, ISkillData> _values;
        private readonly Dictionary<ActionId, IActionData> _actions;

        public SkillDataBase()
        {
            _values = new();
            _actions = new();

            foreach (SkillData data in Resources.LoadAll<SkillData>("Temp/TestSkills"))
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

            foreach (IActionData action in value.AssociatedActions)
            {
                _actions[action.Id] = action;
            }
        }

        public ISkillData Get(SkillId id) => _values[id];

        public IFrameData GetFrameData(ActionId id)
        {
            if (_actions.TryGetValue(id, out IActionData data) == false)
            {
                return null;
            }

            return data.FrameData;
        }

        public AnimationClip GetAnimation(ActionId id)
        {
            if (_actions.TryGetValue(id, out IActionData data) == false)
            {
                return null;
            }

            return data.Animation;
        }

        public void Free(SkillId id) => _values.Remove(id);

        public bool IsLoaded(SkillId id) => _values.ContainsKey(id);

        public IReadOnlyCollection<ActionId> GetAssociatedActions(SkillId id)
        {
            if (_actions.ContainsKey(new(id.Value)) == false)
            {
                return Array.Empty<ActionId>();
            }

            return new ActionId[] { new(id.Value) };
        }
    }
}
