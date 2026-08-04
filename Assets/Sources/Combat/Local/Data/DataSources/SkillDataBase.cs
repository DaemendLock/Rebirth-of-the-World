using Combat.API.Scripting;
using Combat.Common.Flags;
using Combat.Common.ValueObjects;
using Combat.Local.Data.Models;
using Combat.Local.Gateways.DataSources;
using Combat.Local.Scripting.Idk;

using Data.Skills.Components;

using System;
using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Combat.Local.Data.Databases
{
    public class SkillDataBase : ISkillDataBase, IActionDataContainer, ISkillScriptTypeProvider
    {
        private readonly SkillStrategyTypeDataSource _skillStrategyTypeProvider;

        private readonly Dictionary<SkillId, global::Data.Entities.SkillData> _values;
        private readonly Dictionary<ActionId, ActionData> _actions;
        private readonly Dictionary<SkillId, string> _skillScripts;

        public SkillDataBase()
        {
            _values = new();
            _actions = new();
            _skillScripts = new();

            _skillStrategyTypeProvider = new SkillStrategyTypeDataSource(typeof(SkillScript));

            foreach (Type type in AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes()).Where(value => typeof(SkillScript).IsAssignableFrom(value)))
            {
                _skillStrategyTypeProvider.Register(type);
            }

            foreach (var data in Resources.LoadAll<global::Data.Entities.SkillData>("Temp/TestSkills"))
            {
                Load(data);
            }
        }

        public void Load(global::Data.Entities.SkillData value)
        {
            SkillId id = value.Id;

            if (IsLoaded(id))
            {
                return;
            }

            _values[value.Id] = value;

            if (value.TryGetComponent(out SkillActionsComponent actions))
            {
                foreach (var action in actions.Values)
                {
                    _actions[action.Id] = new(action.Animation, action.FrameData);
                }
            }

            if (value.TryGetComponent(out SkillScriptComponent script))
            {
                _skillScripts[id] = script.ScriptName;
            }
        }

        public global::Data.Entities.SkillData Get(SkillId id) => _values[id];

        public SkillFlags GetDefaultFlags(SkillId id) => _values[id].Flags;

        public Type GetScriptType(SkillId id)
        {
            if (_skillScripts.TryGetValue(id, out string name) == false)
            {
                return null;
            }

            if (_skillStrategyTypeProvider.TryGet(name, out Type result) == false)
            {
                return null;
            }

            return result;
        }

        public bool TryGetActionData(ActionId id, out ActionData value)
        {
            if (_actions.TryGetValue(id, out ActionData data) == false)
            {
                value = default;
                return false;
            }

            value = data;
            return true;
        }

        public void Free(SkillId id) => _values.Remove(id);

        public bool IsLoaded(SkillId id) => _values.ContainsKey(id);

        public IReadOnlyCollection<ActionId> GetAssociatedActions(SkillId id)
        {
            if (_values.TryGetValue(id, out var data) == false)
            {
                return Array.Empty<ActionId>();
            }

            if (data.TryGetComponent(out SkillActionsComponent actions) == false)
            {
                return Array.Empty<ActionId>();
            }

            return actions.Values.Select(value => value.Id).ToArray();
        }

        public void Register(ActionId id, ActionData data) => _actions.Add(id, data);

        public ActionData Get(ActionId id) => _actions[id];
    }
}
