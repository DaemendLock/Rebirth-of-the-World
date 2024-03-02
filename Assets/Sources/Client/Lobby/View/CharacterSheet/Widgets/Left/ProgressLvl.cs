using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.DataTypes;

namespace Client.Lobby.View.CharacterSheet
{
    public class ProgressLvl : MonoBehaviour
    {
        [SerializeField] private TMP_Text _progress;
        [SerializeField] private Image _progressBar;

        public ProgressValue Value
        {
            set
            {
                _progress.text = $"Lv. {value.Level}/{value.MaxLevel}";
                _progressBar.fillAmount = (float) value.CurrentProgress / value.MaxProgression;
            }
        }
    }
}
