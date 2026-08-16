using Lobby.Local.Domain.UseCases.Scenarios;
using Lobby.Local.Domain.ValueObjects;
using Lobby.Local.Presentation.ViewModels;

using TMPro;

using UnityEngine;
using UnityEngine.EventSystems;

using Zenject;

namespace Lobby.Local.Presentation.View
{
    public sealed class CharacterSheetView : MonoBehaviour, IPointerClickHandler
    {
        [SerializeField] private TMP_Text _characterName;
        [SerializeField] private LevelWidget _levelWidget;
        [SerializeField] private GameObject _selectButton;

        [Inject] private ScenarioSelectCharacterUseCase _scenarioSelectCharacterUseCase;

        private void Start()
        {
            Show(new()
            {
                LocalizedName = "Katkat",
                Level = new(20, 80, 50, 100)
            });
        }

        public void Show(CharacterViewModel viewModel)
        {
            _characterName.text = viewModel.LocalizedName;

            Level level = viewModel.Level;
            _levelWidget.Level = (level.CurrentValue, level.MaxValue);
            _levelWidget.Progress = (level.CurrentProgress, level.TargetProgress);
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
                    _scenarioSelectCharacterUseCase.Execute(default, default);
                }
            }
        }
    }
}
