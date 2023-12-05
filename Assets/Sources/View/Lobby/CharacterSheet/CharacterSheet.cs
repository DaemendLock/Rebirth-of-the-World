using Core.Lobby.Characters;
using Data.Characters;
using UnityEngine;
using UnityEngine.EventSystems;
using View.Lobby.CharacterSheet.Widgets;
using View.Lobby.CharacterSheet.Widgets.Left;
using View.Lobby.TeamSetup.Widgets;
using View.Lobby.Utils;

namespace View.Lobby.CharacterSheet
{
    public enum ViewMode
    {
        View,
        Edit,
        Select
    }

    public class CharacterSheet : MenuElement
    {
        [HideInInspector] public CharacterSlotWidget SelectionTarget;
        public ViewMode ViewMode = ViewMode.Edit;

        private Character _unit;
        private CharacterState _data;

        [Header("Widgets")]
        [Header("Left")]
        [SerializeField] private GearWidget _gear;
        [Header("Right")]
        [SerializeField] private DescriptionWidget _description;
        [SerializeField] private AffectionWidget _affection;
        [SerializeField] private SpellListWidget _spells;
        [SerializeField] private StatsWidget _stats;
        [SerializeField] private General.Button _selectButton;

        private void OnEnable()
        {
            if (ViewMode != ViewMode.Select)
            {
                _selectButton.gameObject.SetActive(false);
                return;
            }

            _selectButton.gameObject.SetActive(true);
            _selectButton.Clicked += SelectCharacter;
        }

        private void OnDisable()
        {
            if (ViewMode == ViewMode.Select)
            {
                _selectButton.Clicked -= SelectCharacter;
            }
        }

        public void InsertCharacter(Character unit, CharacterState data)
        {
            _unit = unit;
            _data = data;

            _gear.ShowEquip(data, ViewMode != ViewMode.View);
            _description.ShowUnit(unit, data);

            _affection.Value = data.Affection;
            _spells.ShowSpells(unit.GetSpellIcons(data.ViewSet));
            _stats.ShowStats(unit.GetStatsTable(data.Level.Level), data.GetGearStats());
        }

        public void ToggleGear()
        {
            //showHideGear.SetBool("Show", !showHideGear.GetBool("Show"));
        }

        private void SelectCharacter(PointerEventData data)
        {
            SelectionTarget.ShowCharacter(_unit, _data);
            Lobby.Instance.GoBack();
            Lobby.Instance.GoBack();
        }
    }
}