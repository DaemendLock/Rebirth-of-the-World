using Lobby.Common.Primitives;
using Lobby.Local.Presentation.Controllers;
using Lobby.Local.Presentation.Misc;
using Lobby.Local.Presentation.ViewModels;

using System.Collections.Generic;

using UnityEngine;
using UnityEngine.EventSystems;

namespace Lobby.Local.Presentation.View
{
    public sealed class CharacterGalleryView : MonoBehaviour, IPointerClickHandler
    {
        [Zenject.Inject] private readonly CharacterGalleryController _controller;
        [Zenject.Inject] private readonly IAssetProvider _assetProvider;

        private readonly Dictionary<CharacterKey, CharacterCardWidget> _widgets = new();

        [SerializeField] private Transform _charcterBoxParent;
        [SerializeField] private CharacterCardWidget _widgetPrefab;

        private void Start()
        {
            foreach (var val in _controller.LoadAll())
            {
                Show(new(val)
                {
                    IsAvailable = true,
                });
            }
        }

        public void Show(CharacterCardViewModel viewModel)
        {
            if (_widgets.TryGetValue(viewModel.CharacterId, out CharacterCardWidget widget) == false)
            {
                widget = Instantiate(_widgetPrefab, _charcterBoxParent);
                _widgets.Add(viewModel.CharacterId, widget);
            }

            widget.Name = viewModel.CharacterId.ToString();
            widget.CharacterArt = _assetProvider.GetCharacterIcon(viewModel.CharacterId);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            CharacterKey? characterId = GetClickedCharacter(eventData);

            if (characterId.HasValue == false)
            {
                return;
            }

            _controller.OpenCharacterSheet(characterId.Value);
        }

        private CharacterKey? GetClickedCharacter(PointerEventData eventData)
        {
            foreach (var widget in _widgets)
            {
                if (eventData.hovered.Contains(widget.Value.gameObject) == false)
                {
                    continue;
                }

                return widget.Key;
            }

            return null;
        }
    }
}