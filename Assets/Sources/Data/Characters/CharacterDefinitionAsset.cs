using Combat.Common.Primitives;

using System;

using UnityEngine;

namespace Data.Characters
{
    [CreateAssetMenu(menuName = "Assets/Characters/Lobby Character")]
    public sealed class CharacterDefinitionAsset : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ViewSet DefaultViewSet { get; private set; }
        [field: SerializeField] public CharacterAttributeInfo[] Attributes { get; private set; }

        public CharacterKey Id => new(name);
    }

    [Serializable]
    public sealed class ViewSet
    {
        [field: SerializeField] public Sprite GalleryIcon { get; private set; }
        [field: SerializeField] public Sprite SheetPhoto { get; private set; }
        [field: SerializeField] public GameObject Model { get; private set; }
    }

    [Serializable]
    public sealed class CharacterResourceInfo
    {
        [SerializeField] private int _id;
        [field: SerializeField, Min(0)] public float MaxValue { get; private set; }

        public ResourceId ResourceId => new(_id);
    }

    [Serializable]
    public sealed class CharacterAttributeInfo
    {
        [field: SerializeField] public Combat.Common.ValueObjects.UnitAttribute Type { get; private set; }
        [field: SerializeField] public float InitialValue { get; private set; }
        [field: SerializeField] public float LevelBonus { get; private set; }
    }
}
