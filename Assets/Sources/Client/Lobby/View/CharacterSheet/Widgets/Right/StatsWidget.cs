using Client.Lobby.Core.Characters;
using Client.Lobby.View.Common.CoreViews;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using Utils.DataStructure;
using View.Lobby.CharacterSheet.Widgets;

namespace Client.Lobby.View.CharacterSheet.Widgets
{
    internal class StatsWidget : CharacterView, IPointerClickHandler
    {
        private StatsTable _baseStats = StatsTable.UNIT_DEFAULT;
        private StatsTable _gearStats = new();
        //[SerializeField] private TMP_Text _statsLine;

        [SerializeField] private StatValueWidget _statValuePrefab;

        private List<StatValueWidget> _values = new();

        private void Awake()
        {
            for (int i = 0; i < StatsTable.STATS_COUNT; i++)
            {
                StatValueWidget widget = Instantiate(_statValuePrefab, transform);
                _values.Add(widget);
                widget.ShowStat((UnitStat) i);
            }
        }

        public override void SetCharacter(Character character)
        {
            return;
            

            _baseStats = character.Stats.BaseStats;
            _gearStats = character.Stats.BonusStats;

            for (int i = 0; i < _values.Count; i++)
            {
                _values[i].ShowValue(_baseStats[i], _gearStats[i]);
            }
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            for (int i = 0; i < StatsTable.STATS_COUNT; i++)
            {
                Debug.Log($"{(UnitStat) i}: {_baseStats[i].BaseValue}/({_baseStats[i].Percent - 100}%) + {_gearStats[i].BaseValue}/({_gearStats[i].Percent}%)");
            }
        }
    }
}
