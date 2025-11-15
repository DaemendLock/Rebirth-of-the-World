using Combat.Common.Flags;
using Combat.Common.ValueObjects;

using System;
using System.Collections.Generic;

using UnityEngine;

namespace Data.Entities
{
    [CreateAssetMenu(menuName = "Assets/Skills/Skill")]
    public class SkillData : ScriptableObject, ISkillData
    {
        [SerializeField] private int _id;
        [field: SerializeField] public float Cooldown { get; private set; }
        [field: SerializeField] public SkillFlags Flags { get; private set; }
        [field: SerializeField] public string ScriptName { get; private set; }

        [field: SerializeField] private ActionData[] _actions;

        public SkillId Id => new(_id);

        public IReadOnlyCollection<IActionData> AssociatedActions => _actions == null ? Array.Empty<IActionData>() : _actions;
    }
    public interface ISkillData
    {
        SkillId Id { get; }
        float Cooldown { get; }
        SkillFlags Flags { get; }
        string ScriptName { get; }
        IReadOnlyCollection<IActionData> AssociatedActions { get; }
    }
}
