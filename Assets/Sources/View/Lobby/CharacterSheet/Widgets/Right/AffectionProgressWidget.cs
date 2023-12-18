using Data.Characters.Lobby;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.Lobby.CharacterSheet.Widgets
{
    internal class AffectionProgressWidget : MonoBehaviour
    {
        [SerializeField] private Image _progressBar;
        [SerializeField] private TMP_Text _value;

        public ProgressValue Value
        {
            set
            {
                _progressBar.fillAmount = (float) value.CurrentProgress / value.MaxProgression;
                _value.text = $"Affection: {value.CurrentProgress}/{value.MaxProgression}";
            }
        }
    }
}
