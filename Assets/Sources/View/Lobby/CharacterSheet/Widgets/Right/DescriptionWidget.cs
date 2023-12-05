using Core.Lobby.Characters;
using Data.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace View.Lobby.CharacterSheet.Widgets
{
    internal class DescriptionWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _unitName;
        [SerializeField] private Image _role;
        [SerializeField] private ProgressLvl _level;

        public void ShowUnit(Character unit, CharacterState data)
        {
            _unitName.text = unit.LocalizedName;
            _level.Value = data.Level;
        }
    }
}
