using Data.Characters.Lobby;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.Lobby.CharacterSheet
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
                _progressBar.fillAmount = value.CurrentProgress / value.MaxProgression;
            }
        }
    }
}
