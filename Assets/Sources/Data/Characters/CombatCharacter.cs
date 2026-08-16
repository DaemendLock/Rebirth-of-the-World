using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

using Data.Entities;

using System;

using UnityEngine;

namespace Data.Characters
{
    [CreateAssetMenu(menuName = "Assets/Characters/Combat Character")]
    public sealed class CombatCharacter : ScriptableObject
    {
        [SerializeField] private string _modelName;

        [field: SerializeField] public float BaseHealth { get; private set; }
        [field: SerializeField] public AttributeInfo[] AttributeInfo { get; private set; } = Array.Empty<AttributeInfo>();
        [field: SerializeField] public CharacterResourceInfo[] Resources { get; private set; } = Array.Empty<CharacterResourceInfo>();
        [field: SerializeField] public SkillData[] Skills { get; private set; } = Array.Empty<SkillData>();

        public CharacterKey CharacterKey => new(name);

        public ModelName ModelName => new(_modelName);
    }

    [Serializable]
    public sealed class CharacterResourceInfo
    {
        [SerializeField] private int _id;
        [field: SerializeField, Min(0)] public float MaxValue { get; private set; }

        public ResourceId ResourceId => new(_id);
    }

    [Serializable]
    public sealed class AttributeInfo
    {
        [field: SerializeField] public UnitAttribute Attribute { get; private set; }
        [field: SerializeField, Min(0)] public float Value { get; private set; }
    }
}
