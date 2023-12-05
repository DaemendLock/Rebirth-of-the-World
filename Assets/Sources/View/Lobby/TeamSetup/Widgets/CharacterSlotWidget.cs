using Core.Lobby.Characters;
using Data.Characters;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using Utils.DataStructure;
using Utils.DataTypes;
using View.Lobby.CharacterSheet;

namespace View.Lobby.TeamSetup.Widgets
{
    public class CharacterSlotWidget : MonoBehaviour, IPointerClickHandler
    {
        public bool AllowEdit = true;

        [SerializeField] private Image _image;
        [SerializeField] private TMP_Text _name;

        private Character _character;
        private CharacterState _data;

        public void ShowCharacter(Character character, CharacterState data)
        {
            _character = character;
            _data = data;

            _image.sprite = character.GetCharacterCard(data.ViewSet);
            _name.text = character.LocalizedName;
        }

        public void OnPointerClick(PointerEventData eventData)
        {
            if (AllowEdit)
            {
                Lobby.Instance?.OpenCharacterSelection(this);
                return;
            }

            if (_character == null)
            {
                return;
            }

            Lobby.Instance.CharacterSheetMode = ViewMode.View;
            Lobby.Instance?.ViewCharacter(_character, _data);
        }

        internal UnitCreationData GetCharacterData(int index)
        {
            StatsTable stats = _character.GetStatsTable(_data.Level.Level) + _data.GetGearStats();

            SpellId[] gearSpells = _data.GetGearSpells();
            SpellId[] spellIds = new SpellId[_data.Spells.Length + gearSpells.Length];
            _data.Spells.CopyTo(spellIds, 0);
            gearSpells.CopyTo(spellIds, _data.Spells.Length);

            UnitCreationData.ModelData mdata = new UnitCreationData.ModelData(spellIds, stats, new UnitCreationData.PositionData(), _character.CastResources,
                (byte) (index % 2));
            UnitCreationData.ViewData vdata = new UnitCreationData.ViewData(_character.Id, _data.ViewSet);
            UnitCreationData udata = new UnitCreationData(index, mdata, vdata);

            return udata;

        }
    }
}
