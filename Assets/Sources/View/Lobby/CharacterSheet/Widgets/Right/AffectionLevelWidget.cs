using Data.Characters.Lobby;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.Lobby.CharacterSheet.Widgets.Right
{
    internal class AffectionLevelWidget : MonoBehaviour
    {
        [SerializeField] private Image _frame;
        [SerializeField] private TMP_Text _value;

        public ProgressValue Value
        {
            set
            {
                _value.text = value.Level.ToString();
            }
        }
    }
}
