using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Lobby.Local.Presentation.View
{
    public sealed class LevelWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _fullLevelText;
        [SerializeField] private Image _progreessBar;

        public (int Current, int Max) Level { set => _fullLevelText.text = $"Lv. {value.Current}/{value.Max}"; }

        public (int Current, int Max) Progress { set => _progreessBar.fillAmount = value.Max == 0 ? 1 : (float)value.Current / value.Max; }
    }
}
