using Core.Lobby.Accounts;
using System.Collections.Generic;
using UnityEngine;

namespace View.Lobby.General.Party
{
    internal class PartyWidget : MonoBehaviour
    {
        private PartyMemberWidget _prefab;
        private List<PartyMemberWidget> _members;

        public void AddPlayer(Account player)
        {
            PartyMemberWidget member = Instantiate(_prefab, transform);
            _members.Add(member);
            member.Init(player);
        }

        public void RemovePlayer(Account player)
        {

        }
    }
}
