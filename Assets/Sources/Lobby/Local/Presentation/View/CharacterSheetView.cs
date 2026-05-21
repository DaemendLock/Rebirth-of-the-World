using Lobby.Local.Domain.ValueObjects;
using Lobby.Local.Presentation.ViewModels;

using TMPro;

using UnityEngine;

namespace Lobby.Local.Presentation.View
{
    public sealed class CharacterSheetView : MonoBehaviour
    {
        [SerializeField] private TMP_Text _characterName;
        [SerializeField] private LevelWidget _levelWidget;

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
    }
}
