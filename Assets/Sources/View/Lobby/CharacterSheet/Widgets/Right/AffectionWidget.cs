using Data.Characters.Lobby;
using UnityEngine;
using View.Lobby.CharacterSheet.Widgets.Right;

namespace View.Lobby.CharacterSheet.Widgets
{
    internal class AffectionWidget : MonoBehaviour
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
    }
}
