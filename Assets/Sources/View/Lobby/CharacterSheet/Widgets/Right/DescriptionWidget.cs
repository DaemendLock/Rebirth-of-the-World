using Data.Localization;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using View.Lobby.General.Charaters;

namespace View.Lobby.CharacterSheet.Widgets
{
    internal class DescriptionWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _unitName;
        [SerializeField] private Image _role;
        [SerializeField] private ProgressLvl _level;
        [SerializeField] private ProgressLvl _affection;

        public void ShowUnit(Character unit)
        {
            _unitName.name = Localization.GetValue(unit.NameToken);
            //_level.Value = unit.Level;
            //_affection.Value = unit.Affection;
        }
    }
}
