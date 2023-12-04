using System;
using UnityEngine;

namespace Core.Lobby.Encounters
{
    [Serializable]
    public class Encounter
    {
        [SerializeField] private string _name;
        [SerializeField] private int _playerCount;

        public Encounter(string name, int playerCount)
        {
            _name = name;
            _playerCount = playerCount;
        }

        public string Name => _name;
        public int PlayerCount => _playerCount;
    }
}
