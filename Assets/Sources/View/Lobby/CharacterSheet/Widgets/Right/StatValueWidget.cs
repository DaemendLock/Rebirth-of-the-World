using Data.Localization;
using TMPro;
using UnityEngine;
using Utils.DataStructure;
using Utils.DataTypes;

namespace View.Lobby.CharacterSheet.Widgets
{
    internal class StatValueWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _statName;
        [SerializeField] private TMP_Text _value;

        public void ShowStat(UnitStat stat)
        {
            name = stat.ToString();
            _statName.text = Localization.GetStatName(stat);
        }

        public void ShowValue(PercentModifiedValue @base, PercentModifiedValue gear)
        {
            _value.text = $"{(@base + gear).CalculatedValue}";
        }
    }
}
