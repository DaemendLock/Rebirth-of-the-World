using TMPro;
using UnityEngine;

namespace View.Lobby.MainMenu.Widgets
{
    internal class ResourceBoardWidget : MonoBehaviour
    {
        [SerializeField] private TMP_Text _goldAmmountText;
        [SerializeField] private TMP_Text _guildTokenAmmountText;

        private uint _goldAmmount;
        private uint _guildToken;

        public uint GoldAmmount
        {
            get => _goldAmmount;
            set
            {
                _goldAmmount = value;
                _goldAmmountText.text = value.ToString();
            }
        }

        public uint GuildTokens
        {
            get => _guildToken;
            set
            {
                _guildToken = value;
                _guildTokenAmmountText.text = value.ToString();
            }
        }
    }
}
