using Lobby.Common.Primitives;
using Lobby.Local.Domain.UseCases.Scenarios;
using Lobby.Local.Domain.ValueObjects;
using Lobby.Local.Presentation.Controllers;
using Lobby.Local.Presentation.Misc;
using Lobby.Local.Presentation.ViewModels;
using Lobby.Local.Presentation.Widgets.CharacterSheet;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

using Zenject;

namespace Lobby.Local.Presentation.View
{
    public sealed class CharacterSheetView : MonoBehaviour, IPointerClickHandler
    {
        [Inject] private readonly CharacterSheetController _controller;
        [Inject] private readonly UiNavigationService _navigationService;

        [SerializeField] private TMP_Text _characterName;
        [SerializeField] private CharacterPhotoWidget _photo;
        [SerializeField] private LevelWidget _levelWidget;
        [SerializeField] private GameObject _selectButton;
        [SerializeField] private LobbyTabWidget _openOnSelect;

        private CharacterKey? _characterId;

        public void Show(CharacterViewModel viewModel, bool allowSelect)
        {
            _characterId = viewModel.CharacterKey;
            _characterName.text = viewModel.LocalizedName;

            _photo.Model = viewModel.CardPhoto;

            Level level = viewModel.Level;
            _levelWidget.Level = (level.CurrentValue, level.MaxValue);
            _levelWidget.Progress = (level.CurrentProgress, level.TargetProgress);

            SetEnableSeletion(allowSelect);
        }

        public void SetEnableSeletion(bool enabled)
        {
            _selectButton.SetActive(enabled);
        }

        void IPointerClickHandler.OnPointerClick(PointerEventData eventData)
        {
            if (eventData.hovered.Contains(_selectButton))
            {
                if (_selectButton.activeSelf)
                {
                    //TODO: Sheet session
                    if (_controller.Select(_characterId))
                    {
                        _navigationService.OpenTab(_openOnSelect);
                    }
                }
            }
        }
    }
}
