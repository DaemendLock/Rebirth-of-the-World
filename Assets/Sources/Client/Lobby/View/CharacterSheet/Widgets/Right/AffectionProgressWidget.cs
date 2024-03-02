using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.DataTypes;

namespace Client.Lobby.View.CharacterSheet.Widgets.Right
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
