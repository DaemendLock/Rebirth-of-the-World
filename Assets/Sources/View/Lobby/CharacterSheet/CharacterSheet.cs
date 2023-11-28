using UnityEngine;
using View.Lobby.CharacterSheet.Widgets;
using View.Lobby.General.Charaters;
using View.Lobby.Utils;

namespace View.Lobby.CharacterSheet
{
    public class CharacterSheet : MenuElement
    {
        private Character _unit;

        [Header("Widgets")]
        [Header("Left")]
        [Header("Right")]
        [SerializeField] private DescriptionWidget _description;
        [SerializeField] private SpellListWidget _spells;
        [SerializeField] private StatsWidget _stats;

        private void OnEnable()
        {
            /*if (insertButton == null) {
                insertButton.text = "Go to trial >";
                //REMOVE
                insertButton.gameObject.SetActive(false);
                return;
            }
            insertButton.text = "Go to trial >";
            //REMOVE
            insertButton.gameObject.SetActive(false);*/
        }

        public void InsertCharacter(Character unit)
        {
            _unit = unit;

            _description.ShowUnit(unit);
            //_spells.ShowSpells(_unit.Spells);
            //_stats.ShowStats(_unit.EvaluateStats());
        }

        public void ToggleGear()
        {
            //showHideGear.SetBool("Show", !showHideGear.GetBool("Show"));
        }
    }
}