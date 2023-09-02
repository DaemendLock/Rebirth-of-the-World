using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.Lobby.Gallery
{
    public class ProgressLvl : MonoBehaviour
    {
        private static readonly byte _maxLevel = 80; //TODO: Load max level from game data

        [SerializeField] private TextMeshProUGUI _progress;
        [SerializeField] private Image _pBar;
        public void SetValue(float val)
        {
            _progress.text = $"Lv. {((int) val)}/{_maxLevel}";
            _pBar.fillAmount = val - (int) val;
        }
    }
}
