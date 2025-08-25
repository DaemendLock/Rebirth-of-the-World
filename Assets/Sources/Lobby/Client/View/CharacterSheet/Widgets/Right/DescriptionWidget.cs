using Client.Lobby.Domain.Characters;
using Client.Lobby.View.Common.CoreViews;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Client.Lobby.View.CharacterSheet.Widgets
{
    internal class DescriptionWidget : CharacterView
    {
        [SerializeField] private TMP_Text _characterName;
        [SerializeField] private Image _characterRole;
        [SerializeField] private ProgressLvl _characterLevel;

        public override void Bind(Character character)
        {
            _characterName.text = character.Info.Name;
            //TODO _characterLevel.Value = character.Progression.Level;
        }
    }
}
