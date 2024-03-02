using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Utils.DataTypes;

namespace Client.Lobby.View.CharacterSheet.Widgets.Right
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
