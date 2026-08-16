using Lobby.Common.Primitives;

using System;

using UnityEngine;

namespace Data.Characters
{
    [CreateAssetMenu(menuName = "Assets/Characters/Lobby Character")]
    public sealed class LobbyCharacter : ScriptableObject
    {
        [field: SerializeField] public string Name { get; private set; }
        [field: SerializeField] public ViewSet DefaultViewSet { get; private set; }

        public CharacterKey Id => new(name);
    }

    [Serializable]
    public sealed class ViewSet
    {
        [field: SerializeField] public Sprite GalleryIcon { get; private set; }
        [field: SerializeField] public Sprite SheetPhoto { get; private set; }
        [field: SerializeField] public GameObject Model { get; private set; }
    }
}
