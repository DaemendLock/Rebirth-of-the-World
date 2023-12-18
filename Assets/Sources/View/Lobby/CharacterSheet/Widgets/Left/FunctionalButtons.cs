using UnityEngine;
using View.Lobby.General;

namespace View.Lobby.CharacterSheet.Widgets
{
    internal class FunctionalButtonsWidgets : MonoBehaviour
    {
        [SerializeField] private Button _pinButton;
        [SerializeField] private Button _showSkinsButton;
        [SerializeField] private Button _hideUiButon;

        private void OnEnable()
        {
            _pinButton.Clicked += _ => PinCharacter();
        }

        private void OnDisable()
        {
            _pinButton.Clicked -= _ => PinCharacter();
        }

        private void PinCharacter()
        {

        }
    }
}