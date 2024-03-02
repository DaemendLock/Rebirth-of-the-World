using Client.Lobby.Core.Characters;
using Client.Lobby.View.CharacterSheet.Widgets.Right;
using Client.Lobby.View.Common.CoreViews;
using UnityEngine;
using Utils.DataTypes;

namespace Client.Lobby.View.CharacterSheet.Widgets
{
    internal class AffectionWidget : CharacterView
    {
        [SerializeField] private AffectionLevelWidget _level;
        [SerializeField] private AffectionProgressWidget _progressBar;

        public ProgressValue Value
        {
            set
            {
                _level.Value = value;
                _progressBar.Value = value;
            }
        }

        public override void SetCharacter(Character value)
        {
            _level.Value = value.Progression.Affection;
            _progressBar.Value = value.Progression.Affection;
        }
    }
}
