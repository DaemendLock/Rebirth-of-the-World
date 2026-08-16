using TMPro;

using UnityEngine;
using UnityEngine.UI;

namespace Lobby.Local.Presentation.Widgets.CharacterSheet
{

    public sealed class AffectionWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _levelText;
        [SerializeField] private Image _progress;

        public int Level { set => _levelText.text = $"Affection: {value}"; }

        public float Progress { set => _progress.fillAmount = value; }
    }
}
