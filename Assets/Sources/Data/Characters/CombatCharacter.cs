using Combat.Common.Primitives;
using Combat.Common.ValueObjects;

using Data.Entities;

using System;

using UnityEngine;

namespace Data.Characters
{
    [CreateAssetMenu(menuName = "Assets/Characters/Comabt Character")]
    public sealed class CombatCharacter : ScriptableObject
    {
        [SerializeField] private string _characterName;
        [SerializeField] private string _modelName;

        [field: SerializeField] public float BaseHealth { get; private set; }
        [field: SerializeField] public AttributeInfo[] AttributeInfo { get; private set; }
        [field: SerializeField] public CharacterResourceInfo[] Resources { get; private set; }
        [field: SerializeField] public SkillData[] Skills { get; private set; }

        public CharacterKey CharacterKey => new(_characterName);

        public ModelName ModelName => new(_modelName);
    }

    [Serializable]
    public sealed class CharacterResourceInfo
    {
        [SerializeField] private int _id;
        [field: SerializeField, Min(0)] public float MaxValue { get; private set; }
    }

    public sealed class AttributeInfo
    {
        [field: SerializeField] public UnitAttribute Attribute { get; private set; }
        [field: SerializeField, Min(0)] public float Value { get; private set; }
    }
}
