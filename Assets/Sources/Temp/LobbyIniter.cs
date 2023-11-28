using Core.Lobby.Characters;
using System.Collections.Generic;
using UnityEngine;

namespace Assets.Sources.Temp
{
    internal class LobbyIniter : MonoBehaviour
    {
        public static List<CharacterData> Characters = new();
        public static List<GameObject> Models = new();

        private void Start()
        {
            //Networking.Networking.Connect("localhost", 27122);

        }
    }
}
