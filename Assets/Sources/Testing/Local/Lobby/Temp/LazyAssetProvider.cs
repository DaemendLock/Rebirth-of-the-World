using Combat.Common.Primitives;

using Data.Characters;

using Lobby.Local.Presentation.Misc;

using System.Collections.Generic;
using System.Linq;

using UnityEngine;

namespace Assets.Sources.Testing.Local.Lobby.Temp
{
    public sealed class LazyAssetProvider : MonoBehaviour, IAssetProvider
    {
        private Dictionary<CharacterKey, CharacterDefinitionAsset> _values;

        private void Start()
        {
            _values = Resources.LoadAll<CharacterDefinitionAsset>("CharacterDefinitions").ToDictionary(value => value.Id);
        }

        public string GetCharacterName(CharacterKey id)
        {
            if (!_values.TryGetValue(id, out CharacterDefinitionAsset character))
            {
                return null;
            }

            return character.Name;
        }

        public Sprite GetCharacterIcon(CharacterKey id)
        {
            if (!_values.TryGetValue(id, out CharacterDefinitionAsset character))
            {
                return null;
            }

            return character.DefaultViewSet.GalleryIcon;
        }

        public Sprite GetCharacterPhoto(CharacterKey id)
        {
            if (!_values.TryGetValue(id, out CharacterDefinitionAsset character))
            {
                return null;
            }

            return character.DefaultViewSet.SheetPhoto;
        }
    }
}
