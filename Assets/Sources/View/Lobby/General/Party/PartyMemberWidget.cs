using Core.Lobby.Accounts;
using UnityEngine;
using UnityEngine.UI;

namespace View.Lobby.General.Party
{
    internal class PartyMemberWidget : MonoBehaviour
    {
        [SerializeField] private Image _profileIcon;

        private Account _player;

        public int AccountId => _player.Id;

        public void Init(Account player)
        {
            _player = player;

        }
    }
}
