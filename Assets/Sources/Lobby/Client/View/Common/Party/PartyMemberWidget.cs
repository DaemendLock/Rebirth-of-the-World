using UnityEngine;
using UnityEngine.UI;

namespace View.Lobby.General.Party
{
    internal class PartyMemberWidget : MonoBehaviour
    {
        [SerializeField] private Image _profileIcon;

        //private AccountData _player;

        public int AccountId { get; private set; }

        public void Init(int id)
        {
            AccountId = id;
            //_player = AccountsDataProvider.Get;

        }
    }
}
